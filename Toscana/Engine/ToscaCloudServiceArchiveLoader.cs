using System;
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

        public ToscaCloudServiceArchiveLoader(IFileSystem fileSystem, IToscaMetadataDeserializer toscaMetadataDeserializer, IToscaSimpleProfileLoader toscaSimpleProfileLoader)
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
                var toscaMetadata = GetToscaMetadata(archive);
                var toscaCloudServiceArchive = new ToscaCloudServiceArchive
                {
                    ToscaMetadata = toscaMetadata
                };
                var archiveEntry = GetZipArchiveEntry(archive, toscaMetadata.EntryDefinitions);
                var toscaSimpleProfile = toscaSimpleProfileLoader.Load(archiveEntry.Open());

                toscaCloudServiceArchive.ToscaSimpleProfiles.Add(toscaMetadata.EntryDefinitions, toscaSimpleProfile);
                return toscaCloudServiceArchive;
            }
        }

        private ToscaMetadata GetToscaMetadata(ZipArchive archive)
        {
            var toscaMetaArchiveEntry = GetZipArchiveEntry(archive, ToscaCloudServiceArchive.ToscaMetaFileName);
            return toscaMetadataDeserializer.Deserialize(toscaMetaArchiveEntry.Open());
        }

        private static ZipArchiveEntry GetZipArchiveEntry(ZipArchive archive, string zipEntryFileName)
        {
            var zipArchiveEntry = archive.Entries.FirstOrDefault(
                a => string.Compare(a.FullName, zipEntryFileName, StringComparison.InvariantCultureIgnoreCase) == 0);

            if (zipArchiveEntry == null)
            {
                throw new FileNotFoundException(
                    string.Format("{0} file not found within TOSCA Cloud Service Archive file.", zipEntryFileName));
            }

            return zipArchiveEntry;
        }
    }
}