using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace Toscana.Engine
{
    internal interface IToscaCloudServiceArchiveSaver
    {
        void Save(ToscaCloudServiceArchive toscaCloudServiceArchive, Stream stream);
    }

    internal class ToscaCloudServiceArchiveSaver : IToscaCloudServiceArchiveSaver
    {
        private readonly IToscaSerializer<ToscaMetadata> metadataSerializer;
        private readonly IToscaSerializer<ToscaServiceTemplate> serviceTemplateSerializer;

        public ToscaCloudServiceArchiveSaver(IToscaSerializer<ToscaMetadata> metadataSerializer, IToscaSerializer<ToscaServiceTemplate> serviceTemplateSerializer)
        {
            this.metadataSerializer = metadataSerializer;
            this.serviceTemplateSerializer = serviceTemplateSerializer;
        }

        /// <summary>
        /// Saves Cloud Service Archive to ZIP file
        /// </summary>
        /// <param name="toscaCloudServiceArchive"></param>
        /// <param name="stream"></param>
        /// <exception cref="ArgumentException">The stream is already closed or does not support reading.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="stream" /> is null.</exception>
        /// <exception cref="InvalidDataException">The contents of the stream are not in the zip archive format.</exception>
        public void Save(ToscaCloudServiceArchive toscaCloudServiceArchive, Stream stream)
        {
            using (var zipArchive = new ZipArchive(stream, ZipArchiveMode.Create, true))
            {
                SaveMetadata(toscaCloudServiceArchive, zipArchive);
                foreach (var serviceTemplate in toscaCloudServiceArchive.ToscaServiceTemplates)
                {
                    SaveServiceTemplates(zipArchive, serviceTemplate);
                }
                foreach (var artifact in toscaCloudServiceArchive.Artifacts)
                {
                    SaveArtifact(zipArchive, artifact.Key, artifact.Value);
                }
            }
        }

        private void SaveArtifact(ZipArchive zipArchive, string filepath, byte[] bytes)
        {
            var serviceTemplateEntry = zipArchive.CreateEntry(filepath);
            using (var writer = new BinaryWriter(serviceTemplateEntry.Open()))
            {
                writer.Write(bytes, 0, bytes.Length);
            }
        }

        private void SaveServiceTemplates(ZipArchive zipArchive, KeyValuePair<string, ToscaServiceTemplate> serviceTemplate)
        {
            var serviceTemplateEntry = zipArchive.CreateEntry(serviceTemplate.Key);
            serviceTemplateSerializer.Serialize(serviceTemplateEntry.Open(), serviceTemplate.Value);
        }

        private void SaveMetadata(ToscaCloudServiceArchive toscaCloudServiceArchive, ZipArchive zipArchive)
        {
            var metadataEntry = zipArchive.CreateEntry(ToscaCloudServiceArchiveLoader.ToscaMetaFileName);
            metadataSerializer.Serialize(metadataEntry.Open(), toscaCloudServiceArchive.ToscaMetadata);
        }
    }
}