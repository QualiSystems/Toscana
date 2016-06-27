using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.IO.Compression;
using System.Linq;

namespace Toscana.Engine
{
    public interface IToscaCloudServiceArchiveLoader
    {
        ToscaCloudServiceArchive Load(string archiveFilePath);
        ToscaCloudServiceArchive Load(Stream archiveStream);
    }

    public class ToscaCloudServiceArchiveLoader : IToscaCloudServiceArchiveLoader
    {
        private readonly IFileSystem fileSystem;
        private readonly IToscaMetadataDeserializer toscaMetadataDeserializer;
        private readonly IToscaSimpleProfileLoader toscaSimpleProfileLoader;

        public ToscaCloudServiceArchiveLoader(IFileSystem fileSystem,
            IToscaMetadataDeserializer toscaMetadataDeserializer, IToscaSimpleProfileLoader toscaSimpleProfileLoader)
        {
            this.fileSystem = fileSystem;
            this.toscaMetadataDeserializer = toscaMetadataDeserializer;
            this.toscaSimpleProfileLoader = toscaSimpleProfileLoader;
        }

        public ToscaCloudServiceArchive Load(string archiveFilePath)
        {
            using (var zipToOpen = fileSystem.File.OpenRead(archiveFilePath))
            {
                return Load(zipToOpen);
            }
        }

        public ToscaCloudServiceArchive Load(Stream archiveStream)
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
                var archiveEntry = GetZipArchiveEntry(Path.Combine(relativePath, toscaMetadata.EntryDefinitions), fillZipArchivesDictionary);
                var toscaSimpleProfile = toscaSimpleProfileLoader.Load(archiveEntry.Open());

                toscaCloudServiceArchive.ToscaSimpleProfiles.Add(toscaMetadata.EntryDefinitions, toscaSimpleProfile);
                return toscaCloudServiceArchive;
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