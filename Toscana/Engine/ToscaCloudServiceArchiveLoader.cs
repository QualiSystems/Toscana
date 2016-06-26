using System;
using System.IO.Abstractions;
using System.IO.Compression;
using System.Linq;

namespace Toscana.Engine
{
    public class ToscaCloudServiceArchiveLoader
    {
        private readonly IFileSystem fileSystem;

        public ToscaCloudServiceArchiveLoader(IFileSystem fileSystem)
        {
            this.fileSystem = fileSystem;
        }

        public ToscaCloudServiceArchive Load(string archiveFilePath)
        {
            using (var zipToOpen = fileSystem.File.OpenRead(archiveFilePath))
            {
                var archive = new ZipArchive(zipToOpen, ZipArchiveMode.Read);
                var zipArchiveEntry =
                    archive.Entries.FirstOrDefault(
                        a => string.Compare(a.Name, "TOSCA.meta", StringComparison.InvariantCultureIgnoreCase) == 0);
                var toscaMetadataDeserializer = new ToscaMetadataDeserializer();
                var toscaMetadata = toscaMetadataDeserializer.Deserialize(zipArchiveEntry.Open());
                var toscaSimpleProfile = ToscaSimpleProfile.Load(toscaMetadata.EntryDefinitions);

                var toscaCloudServiceArchive = new ToscaCloudServiceArchive
                {
                    ToscaMetadata = toscaMetadata
                };
                toscaCloudServiceArchive.ToscaSimpleProfiles.Add(toscaMetadata.EntryDefinitions, toscaSimpleProfile);
                return toscaCloudServiceArchive;
            }
        }
    }
}