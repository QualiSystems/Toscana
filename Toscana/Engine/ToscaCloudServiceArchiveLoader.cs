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
        private readonly IToscaMetadataDeserializer toscaMetadataDeserializer;
        private readonly IToscaServiceTemplateLoader toscaServiceTemplateLoader;

        public ToscaCloudServiceArchiveLoader(IFileSystem fileSystem,
            IToscaMetadataDeserializer toscaMetadataDeserializer, IToscaServiceTemplateLoader toscaServiceTemplateLoader)
        {
            this.fileSystem = fileSystem;
            this.toscaMetadataDeserializer = toscaMetadataDeserializer;
            this.toscaServiceTemplateLoader = toscaServiceTemplateLoader;
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
                var fillZipArchivesDictionary = FillZipArchivesDictionary(archive);
                var toscaMetaArchiveEntry = GetToscaMetaArchiveEntry(fillZipArchivesDictionary);
                var relativePath = Path.GetDirectoryName(toscaMetaArchiveEntry.FullName) ?? string.Empty;
                var toscaMetadata = toscaMetadataDeserializer.Deserialize(toscaMetaArchiveEntry.Open());
                var toscaCloudServiceArchive = new ToscaCloudServiceArchive
                {
                    ToscaMetadata = toscaMetadata
                };
                var toscaSimpleProfile = LoadToscaServiceTemplate(alternativePath, relativePath, toscaMetadata, fillZipArchivesDictionary);

                toscaCloudServiceArchive.ToscaServiceTemplates.Add(toscaMetadata.EntryDefinitions, toscaSimpleProfile);
                return toscaCloudServiceArchive;
            }
        }

        private ToscaServiceTemplate LoadToscaServiceTemplate(string alternativePath, string relativePath,
            ToscaMetadata toscaMetadata, Dictionary<string, ZipArchiveEntry> fillZipArchivesDictionary)
        {
            var zipEntryFileName = Path.Combine(relativePath, toscaMetadata.EntryDefinitions);
            try
            {
                var archiveEntry = GetZipArchiveEntry(zipEntryFileName, fillZipArchivesDictionary);
                return toscaServiceTemplateLoader.Load(archiveEntry.Open(), alternativePath);
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

        private static Dictionary<string, ZipArchiveEntry> FillZipArchivesDictionary(ZipArchive archive)
        {
            var zipArchiveEntries = new Dictionary<string, ZipArchiveEntry>(new PathEqualityComparer());
            foreach (var zipArchiveEntry in archive.Entries)
            {
                zipArchiveEntries.Add(zipArchiveEntry.FullName, zipArchiveEntry);
            }
            return zipArchiveEntries;
        }

        private static ZipArchiveEntry GetZipArchiveEntry(string zipEntryFileName, IReadOnlyDictionary<string, ZipArchiveEntry> zipArchivesDictionary)
        {
            ZipArchiveEntry zipArchiveEntry;
            if (!zipArchivesDictionary.TryGetValue(zipEntryFileName, out zipArchiveEntry))
            {
                throw new FileNotFoundException(
                    string.Format("{0} file not found within TOSCA Cloud Service Archive file.", zipEntryFileName));
            }
            return zipArchiveEntry;
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