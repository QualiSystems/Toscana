using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using Toscana.Common;
using Toscana.Engine;
using Toscana.Exceptions;

namespace Toscana
{
    public class ToscaCloudServiceArchive
    {
        private readonly Dictionary<string, ToscaNodeType> nodeTypes;
        private readonly ToscaMetadata toscaMetadata;
        private readonly Dictionary<string, ToscaServiceTemplate> toscaServiceTemplates;
        private readonly Dictionary<string, byte[]> fileContents;

        public ToscaCloudServiceArchive(ToscaMetadata toscaMetadata, IReadOnlyDictionary<string, ZipArchiveEntry> archiveEntries = null)
        {
            this.toscaMetadata = toscaMetadata;
            toscaServiceTemplates = new Dictionary<string, ToscaServiceTemplate>();
            nodeTypes = new Dictionary<string, ToscaNodeType>();
            if (archiveEntries == null)
            {
                fileContents = new Dictionary<string, byte[]>();
            }
            else
            {
                fileContents = archiveEntries.ToDictionary(a => a.Key, a => a.Value.Open().ReadAllBytes());
            }
        }

        public ToscaServiceTemplate EntryPointServiceTemplate
        {
            get { return toscaServiceTemplates[ToscaMetadata.EntryDefinitions]; }
        }

        public IReadOnlyDictionary<string, ToscaServiceTemplate> ToscaServiceTemplates
        {
            get { return toscaServiceTemplates; }
        }

        public ToscaMetadata ToscaMetadata
        {
            get { return toscaMetadata; }
        }

        public IReadOnlyDictionary<string, ToscaNodeType> NodeTypes
        {
            get { return nodeTypes; }
        }

        /// <summary>
        ///     Loads Cloud Service Archive (CSAR) file and all its dependencies
        /// </summary>
        /// <param name="archiveFilePath">Path to Cloud Service Archive (CSAR) zip file</param>
        /// <param name="alternativePath">Path for dependencies lookup outside the archive</param>
        /// <exception cref="Toscana.Exceptions.ToscaCloudServiceArchiveFileNotFoundException">Thrown when CSAR file not found.</exception>
        /// <exception cref="Toscana.Exceptions.ToscaMetadataFileNotFound">Thrown when TOSCA.meta file not found in the archive.</exception>
        /// <exception cref="Toscana.Exceptions.ToscaImportFileNotFoundException">
        ///     Thrown when import file neither found in the
        ///     archive nor at the alternative path.
        /// </exception>
        /// <returns>A valid instance of ToscaCloudServiceArchive</returns>
        public static ToscaCloudServiceArchive Load(string archiveFilePath, string alternativePath = null)
        {
            var toscaCloudServiceArchiveLoader = GetToscaCloudServiceArchiveLoader();
            return toscaCloudServiceArchiveLoader.Load(archiveFilePath, alternativePath);
        }

        /// <summary>
        ///     Loads Cloud Service Archive (CSAR) file and all its dependencies
        /// </summary>
        /// <param name="archiveStream">Stream to Cloud Service Archive (CSAR) zip file</param>
        /// <param name="alternativePath">Path for dependencies lookup outside the archive</param>
        /// <exception cref="Toscana.Exceptions.ToscaMetadataFileNotFound">Thrown when TOSCA.meta file not found in the archive.</exception>
        /// <exception cref="Toscana.Exceptions.ToscaImportFileNotFoundException">
        ///     Thrown when import file neither found in the
        ///     archive nor at the alternative path.
        /// </exception>
        /// <returns>A valid instance of ToscaCloudServiceArchive</returns>
        public static ToscaCloudServiceArchive Load(Stream archiveStream, string alternativePath = null)
        {
            var toscaCloudServiceArchiveLoader = GetToscaCloudServiceArchiveLoader();
            return toscaCloudServiceArchiveLoader.Load(archiveStream, alternativePath);
        }

        private static IToscaCloudServiceArchiveLoader GetToscaCloudServiceArchiveLoader()
        {
            return new Bootstrapper().GetToscaCloudServiceArchiveLoader();
        }

        /// <summary>
        /// Adds a ToscaServiceTemplate
        /// </summary>
        /// <exception cref="ArtifactNotFoundException">Thrown when artifact not found in the Cloud Service Archive.</exception>
        /// <param name="toscaServiceTemplateName">Service template name</param>
        /// <param name="toscaServiceTemplate">An instance of ToscaServiceTemplate</param>
        public void AddToscaServiceTemplate(string toscaServiceTemplateName, ToscaServiceTemplate toscaServiceTemplate)
        {
            toscaServiceTemplates.Add(toscaServiceTemplateName, toscaServiceTemplate);
            foreach (var toscaNodeType in toscaServiceTemplate.NodeTypes)
            {
                nodeTypes.Add(toscaNodeType.Key, toscaNodeType.Value);
                foreach (var toscaArtifact in toscaNodeType.Value.Artifacts)
                {
                    if (!fileContents.ContainsKey(toscaArtifact.Value.File))
                    {
                        throw new ArtifactNotFoundException(string.Format("Artifact '{0}' not found in Cloud Service Archive.",
                            toscaArtifact.Value.File));
                    }
                }
            }
        }

        /// <summary>
        ///     Returns ToscaNodeTypes from the Entry TOSCA YAML file that are not used in DerivedFrom of other node types,
        ///     e.g. are leaves in node types inheritance.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public IReadOnlyDictionary<string, ToscaNodeType> GetEntryLeafNodeTypes()
        {
            var baseNodeTypes =
                EntryPointServiceTemplate.NodeTypes.Values.Where(n => !n.IsRoot())
                    .Select(n => n.DerivedFrom)
                    .ToHashSet();
            return EntryPointServiceTemplate.NodeTypes.Where(n => !baseNodeTypes.Contains(n.Key))
                .ToDictionary(_ => _.Key, _ => _.Value);
        }

        /// <summary>
        /// Gets file content as byte array
        /// </summary>
        /// <param name="fileName">File name to return content of</param>
        /// <returns>File content as byte array</returns>
        public byte[] GetArtifactBytes(string fileName)
        {
            byte[] content;
            if (!fileContents.TryGetValue(fileName, out content))
            {
                throw new ArtifactNotFoundException(string.Format("Artifact '{0}' not found in Cloud Service Archive.",
                    fileName));
            }
            return content;
        }
    }
}