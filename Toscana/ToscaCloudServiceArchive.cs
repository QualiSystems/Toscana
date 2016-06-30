using System.Collections.Generic;
using System.IO;
using Toscana.Engine;

namespace Toscana
{
    public class ToscaCloudServiceArchive
    {
        private readonly Dictionary<string, ToscaNodeType> nodeTypes;

        public ToscaCloudServiceArchive()
        {
            ToscaServiceTemplates = new Dictionary<string, ToscaServiceTemplate>();
            ToscaMetadata = new ToscaMetadata();
            nodeTypes = new Dictionary<string, ToscaNodeType>();
        }

        public ToscaServiceTemplate EntryPointServiceTemplate
        {
            get { return ToscaServiceTemplates[ToscaMetadata.EntryDefinitions]; }
        }

        public Dictionary<string, ToscaServiceTemplate> ToscaServiceTemplates { get; set; }
        public ToscaMetadata ToscaMetadata { get; set; }

        public IReadOnlyDictionary<string, ToscaNodeType> NodeTypes
        {
            get { return nodeTypes; }
        }

        /// <summary>
        /// Loads Cloud Service Archive (CSAR) file and all its dependencies
        /// </summary>
        /// <param name="archiveFilePath">Path to Cloud Service Archive (CSAR) zip file</param>
        /// <param name="alternativePath">Path for dependencies lookup outside the archive</param>
        /// <exception cref="Toscana.Exceptions.ToscaCloudServiceArchiveFileNotFoundException">Thrown when CSAR file not found.</exception>
        /// <exception cref="Toscana.Exceptions.ToscaMetadataFileNotFound">Thrown when TOSCA.meta file not found in the archive.</exception>
        /// <exception cref="Toscana.Exceptions.ToscaImportFileNotFoundException">Thrown when import file neither found in the archive nor at the alternative path.</exception>
        /// <returns>A valid instance of ToscaCloudServiceArchive</returns>
        public static ToscaCloudServiceArchive Load(string archiveFilePath, string alternativePath = null)
        {
            var toscaCloudServiceArchiveLoader = GetToscaCloudServiceArchiveLoader();
            return toscaCloudServiceArchiveLoader.Load(archiveFilePath, alternativePath);
        }

        /// <summary>
        /// Loads Cloud Service Archive (CSAR) file and all its dependencies
        /// </summary>
        /// <param name="archiveStream">Stream to Cloud Service Archive (CSAR) zip file</param>
        /// <param name="alternativePath">Path for dependencies lookup outside the archive</param>
        /// <exception cref="Toscana.Exceptions.ToscaMetadataFileNotFound">Thrown when TOSCA.meta file not found in the archive.</exception>
        /// <exception cref="Toscana.Exceptions.ToscaImportFileNotFoundException">Thrown when import file neither found in the archive nor at the alternative path.</exception>
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

        public void AddToscaServiceTemplate(string toscaServiceTemplateName, ToscaServiceTemplate toscaServiceTemplate)
        {
            ToscaServiceTemplates.Add(toscaServiceTemplateName, toscaServiceTemplate);
        }

        public void AddToscaNodeType(string toscaNodeTypeName, ToscaNodeType toscaNodeType)
        {
            nodeTypes.Add(toscaNodeTypeName, toscaNodeType);
        }
    }
}