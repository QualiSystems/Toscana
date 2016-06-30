using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.IO.Compression;
using System.Linq;
using Toscana.Common;
using Toscana.Exceptions;

namespace Toscana.Engine
{
    public interface IToscaCloudServiceArchiveLoader
    {
        ToscaCloudServiceArchive Load(string archiveFilePath, string alternativePath = null);
        ToscaCloudServiceArchive Load(Stream archiveStream, string alternativePath = null);
    }

    public class ToscaCloudServiceArchiveLoader : IToscaCloudServiceArchiveLoader
    {
        private readonly IFileSystem fileSystem;
        private readonly IToscaParser<ToscaMetadata> metadataParser;
        private readonly IToscaParser<ToscaServiceTemplate> serviceTemplateParser;

        public ToscaCloudServiceArchiveLoader(IFileSystem fileSystem,
            IToscaParser<ToscaMetadata> metadataParser, IToscaParser<ToscaServiceTemplate> serviceTemplateParser)
        {
            this.fileSystem = fileSystem;
            this.metadataParser = metadataParser;
            this.serviceTemplateParser = serviceTemplateParser;
        }

        public ToscaCloudServiceArchive Load(string archiveFilePath, string alternativePath = null)
        {
            using (var zipToOpen = fileSystem.File.OpenRead(archiveFilePath))
            {
                return Load(zipToOpen, alternativePath);
            }
        }

        public ToscaCloudServiceArchive Load(Stream archiveStream, string alternativePath = null)
        {
            using (var archive = new ZipArchive(archiveStream, ZipArchiveMode.Read))
            {
                var archiveEntries = CreateArchiveEntriesDictionary(archive);
                var toscaMetaArchiveEntry = GetToscaMetaArchiveEntry(archiveEntries);
                var relativePath = Path.GetDirectoryName(toscaMetaArchiveEntry.FullName) ?? string.Empty;
                var toscaMetadata = metadataParser.Parse(toscaMetaArchiveEntry.Open());
                var toscaCloudServiceArchive = new ToscaCloudServiceArchive
                {
                    ToscaMetadata = toscaMetadata
                };
                LoadDependenciesRecursively(toscaCloudServiceArchive, archiveEntries, toscaMetadata.EntryDefinitions, alternativePath, relativePath);
 
                return toscaCloudServiceArchive;
            }
        }

        private void LoadDependenciesRecursively(ToscaCloudServiceArchive toscaCloudServiceArchive, Dictionary<string, ZipArchiveEntry> zipArchiveEntries, string toscaServiceTemplateName, string alternativePath, string relativePath)
        {
            var toscaServiceTemplate = LoadToscaServiceTemplate(alternativePath, relativePath, zipArchiveEntries, toscaServiceTemplateName);
            toscaCloudServiceArchive.AddToscaServiceTemplate(toscaServiceTemplateName, toscaServiceTemplate);
            foreach (var importFile in toscaServiceTemplate.Imports.SelectMany(import => import.Values))
            {
                LoadDependenciesRecursively(toscaCloudServiceArchive, zipArchiveEntries, importFile.File, alternativePath, relativePath);
            }
        }

        private ToscaServiceTemplate LoadToscaServiceTemplate(string alternativePath, string relativePath, Dictionary<string, ZipArchiveEntry> archiveEntries, string filePath)
        {
            var zipEntryFileName = Path.Combine(relativePath, filePath);
            try
            {
                ZipArchiveEntry zipArchiveEntry;
                if (!archiveEntries.TryGetValue(zipEntryFileName, out zipArchiveEntry))
                {
                    if (!string.IsNullOrEmpty(alternativePath))
                    {
                        var alternativeFullPath = fileSystem.Path.Combine(alternativePath, zipEntryFileName);
                        if ( !fileSystem.File.Exists(alternativeFullPath))
                        {
                            throw new FileNotFoundException(string.Format("{0} file neither found within TOSCA Cloud Service Archive nor at alternative location '{1}'.", zipEntryFileName , alternativePath));
                        }
                        return serviceTemplateParser.Parse(fileSystem.File.OpenRead(alternativeFullPath));
                    }
                    throw new FileNotFoundException(string.Format("{0} file not found within TOSCA Cloud Service Archive file.", zipEntryFileName));
                }
                return serviceTemplateParser.Parse(zipArchiveEntry.Open());
            }
            catch (ToscaBaseException toscaBaseException)
            {
                throw new ToscaParsingException(
                    string.Format("Failed to load definitions file {0} due to an error: {1}", zipEntryFileName, toscaBaseException.GetaAllMessages()));
            }
        }

        private static ZipArchiveEntry GetToscaMetaArchiveEntry(Dictionary<string, ZipArchiveEntry> fillZipArchivesDictionary)
        {
            var toscaMetaArchiveEntry = fillZipArchivesDictionary.Values.FirstOrDefault(
                a =>
                    string.Compare(a.Name, ToscaCloudServiceArchive.ToscaMetaFileName,
                        StringComparison.InvariantCultureIgnoreCase) == 0);
            if (toscaMetaArchiveEntry == null)
            {
                throw new FileNotFoundException(
                    string.Format("{0} file not found within TOSCA Cloud Service Archive file.", ToscaCloudServiceArchive.ToscaMetaFileName)); 
            }
            return toscaMetaArchiveEntry;
        }

        private static Dictionary<string, ZipArchiveEntry> CreateArchiveEntriesDictionary(ZipArchive archive)
        {
            var zipArchiveEntries = new Dictionary<string, ZipArchiveEntry>(new PathEqualityComparer());
            foreach (var zipArchiveEntry in archive.Entries)
            {
                zipArchiveEntries.Add(zipArchiveEntry.FullName, zipArchiveEntry);
            }
            return zipArchiveEntries;
        }
    }

    public class PathEqualityComparer : IEqualityComparer<string>
    {
        public bool Equals(string x, string y)
        {
            if (string.Compare(x, y, StringComparison.InvariantCultureIgnoreCase) == 0)
            {
                return true;
            }
            if (string.Compare(NormalizePath(x), NormalizePath(y), StringComparison.InvariantCultureIgnoreCase) == 0)
            {
                return true;
            }
            if (string.Compare(Path.GetFileName(x), Path.GetFileName(y), StringComparison.InvariantCultureIgnoreCase) == 0)
            {
                return true;
            }
            return false;
        }

        public int GetHashCode(string obj)
        {
            return NormalizePath(obj).GetHashCode();
        }

        private static string NormalizePath(string unnormalized)
        {
            return unnormalized.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
        }
    }
}