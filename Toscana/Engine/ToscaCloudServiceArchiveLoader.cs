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
    internal interface IToscaCloudServiceArchiveLoader
    {
        /// <summary>
        /// Loads Cloud Service Archive (CSAR) file and all its dependencies
        /// </summary>
        /// <param name="archiveFilePath">Path to Cloud Service Archive (CSAR) zip file</param>
        /// <param name="alternativePath">Path for dependencies lookup outside the archive</param>
        /// <exception cref="Toscana.Exceptions.ToscaCloudServiceArchiveFileNotFoundException">Thrown when CSAR file not found.</exception>
        /// <exception cref="ToscaMetadataFileNotFoundException">Thrown when TOSCA.meta file not found in the archive.</exception>
        /// <exception cref="Toscana.Exceptions.ToscaImportFileNotFoundException">Thrown when import file neither found in the archive nor at the alternative path.</exception>
        /// <returns>A valid instance of ToscaCloudServiceArchive</returns>
        ToscaCloudServiceArchive Load(string archiveFilePath, string alternativePath = null);

        /// <summary>
        /// Loads Cloud Service Archive (CSAR) file and all its dependencies
        /// </summary>
        /// <param name="archiveStream">Stream to Cloud Service Archive (CSAR) zip file</param>
        /// <param name="alternativePath">Path for dependencies lookup outside the archive</param>
        /// <exception cref="ToscaMetadataFileNotFoundException">Thrown when TOSCA.meta file not found in the archive.</exception>
        /// <exception cref="Toscana.Exceptions.ToscaImportFileNotFoundException">Thrown when import file neither found in the archive nor at the alternative path.</exception>
        /// <returns>A valid instance of ToscaCloudServiceArchive</returns>
        ToscaCloudServiceArchive Load(Stream archiveStream, string alternativePath = null);
    }

    internal class ToscaCloudServiceArchiveLoader : IToscaCloudServiceArchiveLoader
    {
        private readonly IFileSystem fileSystem;
        private readonly IToscaParser<ToscaMetadata> metadataParser;
        private readonly IToscaParser<ToscaServiceTemplate> serviceTemplateParser;
        private readonly IToscaValidator<ToscaCloudServiceArchive> validator;

        public ToscaCloudServiceArchiveLoader(IFileSystem fileSystem,
            IToscaParser<ToscaMetadata> metadataParser, IToscaParser<ToscaServiceTemplate> serviceTemplateParser, IToscaValidator<ToscaCloudServiceArchive> validator)
        {
            this.fileSystem = fileSystem;
            this.metadataParser = metadataParser;
            this.serviceTemplateParser = serviceTemplateParser;
            this.validator = validator;
        }

        /// <summary>
        /// Loads Cloud Service Archive (CSAR) file and all its dependencies
        /// </summary>
        /// <param name="archiveFilePath">Path to Cloud Service Archive (CSAR) zip file</param>
        /// <param name="alternativePath">Path for dependencies lookup outside the archive</param>
        /// <exception cref="Toscana.Exceptions.ToscaCloudServiceArchiveFileNotFoundException">Thrown when CSAR file not found.</exception>
        /// <exception cref="ToscaMetadataFileNotFoundException">Thrown when TOSCA.meta file not found in the archive.</exception>
        /// <exception cref="Toscana.Exceptions.ToscaImportFileNotFoundException">Thrown when import file neither found in the archive nor at the alternative path.</exception>
        /// <returns>A valid instance of ToscaCloudServiceArchive</returns>
        public ToscaCloudServiceArchive Load(string archiveFilePath, string alternativePath = null)
        {
            if (!fileSystem.File.Exists(archiveFilePath))
            {
                throw new ToscaCloudServiceArchiveFileNotFoundException(string.Format("Cloud Service Archive (CSAR) file '{0}' not found", archiveFilePath));
            }
            using (var zipToOpen = fileSystem.File.OpenRead(archiveFilePath))
            {
                return Load(zipToOpen, alternativePath);
            }
        }

        /// <summary>
        /// Loads Cloud Service Archive (CSAR) file and all its dependencies
        /// </summary>
        /// <param name="archiveStream">Stream to Cloud Service Archive (CSAR) zip file</param>
        /// <param name="alternativePath">Path for dependencies lookup outside the archive</param>
        /// <exception cref="ToscaMetadataFileNotFoundException">Thrown when TOSCA.meta file not found in the archive.</exception>
        /// <exception cref="Toscana.Exceptions.ToscaImportFileNotFoundException">Thrown when import file neither found in the archive nor at the alternative path.</exception>
        /// <exception cref="InvalidDataException">The contents of the stream could not be interpreted as a zip archive or the archive is corrupt and cannot be read.</exception>
        /// <returns>A valid instance of ToscaCloudServiceArchive</returns>
        public ToscaCloudServiceArchive Load(Stream archiveStream, string alternativePath = null)
        {
            using (var archive = CreateZipArchiveFromStream(archiveStream))
            {
                var archiveEntries = archive.GetArchiveEntriesDictionary();
                var toscaMetaArchiveEntry = GetToscaMetaArchiveEntry(archiveEntries);
                var toscaMetadata = metadataParser.Parse(toscaMetaArchiveEntry.Open());
                var toscaCloudServiceArchive = new ToscaCloudServiceArchive(toscaMetadata);
                LoadDependenciesRecursively(toscaCloudServiceArchive, archiveEntries, toscaMetadata.EntryDefinitions, alternativePath);
                validator.Validate(toscaCloudServiceArchive);
                return toscaCloudServiceArchive;
            }
        }

        private static ZipArchive CreateZipArchiveFromStream(Stream archiveStream)
        {
            try
            {
                return new ZipArchive(archiveStream, ZipArchiveMode.Read);
            }
            catch (InvalidDataException ioException)
            {
                throw new ToscaInvalidFileException("Zip files are the only file format supported", ioException);
            }
        }

        private void LoadDependenciesRecursively(ToscaCloudServiceArchive cloudServiceArchive, IReadOnlyDictionary<string, ZipArchiveEntry> zipArchiveEntries, string serviceTemplateName, string alternativePath)
        {
            if (cloudServiceArchive.ToscaServiceTemplates.ContainsKey(serviceTemplateName)) return;

            var serviceTemplate = LoadToscaServiceTemplate(alternativePath, zipArchiveEntries, serviceTemplateName, cloudServiceArchive);
            cloudServiceArchive.AddToscaServiceTemplate(serviceTemplateName, serviceTemplate);
            foreach (var nodeType in serviceTemplate.NodeTypes)
            {
                foreach (var artifact in nodeType.Value.Artifacts)
                {
                    ZipArchiveEntry zipArchiveEntry;
                    var artifactPath = artifact.Value.File;
                    if (zipArchiveEntries.TryGetValue(artifactPath, out zipArchiveEntry))
                    {
                        cloudServiceArchive.AddArtifact(artifactPath, zipArchiveEntry.Open().ReadAllBytes());
                    }
                    if (alternativePath != null)
                    {
                        var artifactFullPath = fileSystem.Path.Combine(alternativePath, artifactPath);
                        if (fileSystem.File.Exists(artifactFullPath))
                        {
                            using (var stream = fileSystem.File.Open(artifactFullPath, FileMode.Open))
                            {
                                cloudServiceArchive.AddArtifact(artifactPath, stream.ReadAllBytes());
                            }
                        }
                    }
                }
            }
            foreach (var importFile in serviceTemplate.Imports.SelectMany(import => import.Values))
            {
                LoadDependenciesRecursively(cloudServiceArchive, zipArchiveEntries, importFile.File, alternativePath);
            }
        }

        private ToscaServiceTemplate LoadToscaServiceTemplate(string alternativePath, IReadOnlyDictionary<string, ZipArchiveEntry> archiveEntries, string filePath, ToscaCloudServiceArchive toscaCloudServiceArchive)
        {
            var importStream = GetImportStream(alternativePath, archiveEntries, filePath);
            try
            {
                return serviceTemplateParser.Parse(importStream);
            }
            catch (ToscaBaseException toscaBaseException)
            {
                throw new ToscaParsingException(
                    string.Format("Failed to load definitions file {0} due to an error: {1}", filePath, toscaBaseException.GetaAllMessages()));
            }
        }

        private Stream GetImportStream(string alternativePath, IReadOnlyDictionary<string, ZipArchiveEntry> archiveEntries, string zipEntryFileName)
        {
            ZipArchiveEntry zipArchiveEntry;
            if (!archiveEntries.TryGetValue(zipEntryFileName, out zipArchiveEntry))
            {
                if (!string.IsNullOrEmpty(alternativePath))
                {
                    var alternativeFullPath = fileSystem.Path.Combine(alternativePath, zipEntryFileName);
                    if (!fileSystem.File.Exists(alternativeFullPath))
                    {
                        throw new ToscaImportFileNotFoundException(
                            string.Format(@"Import file '{0}' neither found within TOSCA Cloud Service Archive nor at alternative location '{1}'",
                                zipEntryFileName, alternativePath));
                    }
                    return fileSystem.File.OpenRead(alternativeFullPath);
                }
                throw new ToscaImportFileNotFoundException(
                    string.Format("Import file '{0}' not found within TOSCA Cloud Service Archive file.", zipEntryFileName));
            }
            return zipArchiveEntry.Open();
        }

        private static ZipArchiveEntry GetToscaMetaArchiveEntry(IReadOnlyDictionary<string, ZipArchiveEntry> fillZipArchivesDictionary)
        {
            var toscaMetaArchiveEntry = fillZipArchivesDictionary.Values.FirstOrDefault(
                a =>
                    string.Compare(a.Name, ToscaMetaFileName,
                        StringComparison.InvariantCultureIgnoreCase) == 0);
            if (toscaMetaArchiveEntry == null)
            {
                throw new ToscaMetadataFileNotFoundException(
                    string.Format("{0} file not found within TOSCA Cloud Service Archive file.", ToscaMetaFileName)); 
            }
            return toscaMetaArchiveEntry;
        }

        public const string ToscaMetaFileName = "TOSCA.meta";
    }
}