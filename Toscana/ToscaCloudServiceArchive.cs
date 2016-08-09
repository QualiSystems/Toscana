using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.IO.Compression;
using System.Linq;
using Toscana.Common;
using Toscana.Engine;
using Toscana.Exceptions;

namespace Toscana
{
    public class ToscaCloudServiceArchive : IValidatableObject
    {
        private readonly Dictionary<string, ToscaNodeType> nodeTypes;
        private readonly ToscaMetadata toscaMetadata;
        private readonly Dictionary<string, ToscaServiceTemplate> toscaServiceTemplates;
        private readonly Dictionary<string, byte[]> fileContents;
        private readonly Dictionary<string, ToscaCapabilityType> capabilityTypes;

        public ToscaCloudServiceArchive(ToscaMetadata toscaMetadata, IReadOnlyDictionary<string, ZipArchiveEntry> archiveEntries = null)
        {
            this.toscaMetadata = toscaMetadata;
            toscaServiceTemplates = new Dictionary<string, ToscaServiceTemplate>();
            nodeTypes = new Dictionary<string, ToscaNodeType>();
            capabilityTypes = new Dictionary<string, ToscaCapabilityType>();
            if (archiveEntries == null)
            {
                fileContents = new Dictionary<string, byte[]>();
            }
            else
            {
                fileContents = new Dictionary<string, byte[]>(new PathEqualityComparer());
                foreach (var archiveEntry in archiveEntries)
                {
                    fileContents.Add(archiveEntry.Value.FullName, archiveEntry.Value.Open().ReadAllBytes());
                }
            }
        }

        public ToscaMetadata ToscaMetadata
        {
            get { return toscaMetadata; }
        }

        public ToscaServiceTemplate GetEntryPointServiceTemplate()
        {
            return toscaServiceTemplates[ToscaMetadata.EntryDefinitions];
        }

        public IReadOnlyDictionary<string, ToscaServiceTemplate> ToscaServiceTemplates
        {
            get { return toscaServiceTemplates; }
        }

        public IReadOnlyDictionary<string, ToscaNodeType> NodeTypes
        {
            get { return nodeTypes; }
        }

        /// <summary>
        /// Returns capability from all the Service Templates 
        /// </summary>
        public IReadOnlyDictionary<string, ToscaCapabilityType> CapabilityTypes
        {
            get { return capabilityTypes; }
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
                AddNodeType(toscaNodeType.Key, toscaNodeType.Value);
            }
            foreach (var capabilityType in toscaServiceTemplate.CapabilityTypes)
            {
                AddCapabilityType(capabilityType.Key, capabilityType.Value);
            }
        }

        private void AddCapabilityType(string capabilityTypeName, ToscaCapabilityType capabilityType)
        {
            capabilityTypes.Add(capabilityTypeName, capabilityType);
            capabilityType.SetToscaCloudServiceArchive(this);
        }

        /// <summary>
        /// Adds a node type with its name
        /// </summary>
        /// <param name="nodeTypeName">Node type name</param>
        /// <param name="nodeType">An instance of node type to add</param>
        /// <exception cref="ArtifactNotFoundException"></exception>
        public void AddNodeType(string nodeTypeName, ToscaNodeType nodeType)
        {
            nodeTypes.Add(nodeTypeName, nodeType);
            nodeType.SetToscaCloudServiceArchive(this);

            foreach (var toscaArtifact in nodeType.Artifacts)
            {
                if (!fileContents.ContainsKey(toscaArtifact.Value.File))
                {
                    throw new ArtifactNotFoundException(String.Format("Artifact '{0}' not found in Cloud Service Archive.",
                        toscaArtifact.Value.File));
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
                GetEntryPointServiceTemplate().NodeTypes.Values.Where(n => !n.IsRoot())
                    .Select(n => n.DerivedFrom)
                    .ToHashSet();
            return GetEntryPointServiceTemplate().NodeTypes.Where(n => !baseNodeTypes.Contains(n.Key))
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
                throw new ArtifactNotFoundException(String.Format("Artifact '{0}' not found in Cloud Service Archive.",
                    fileName));
            }
            return content;
        }

        /// <summary>
        /// If needed adds built-in node types and capabilities
        /// </summary>
        public void FillDefaults()
        {
            if (!NodeTypes.ContainsKey(ToscaDefaults.ToscaNodesRoot))
            {
                AddNodeType(ToscaDefaults.ToscaNodesRoot, ToscaDefaults.GetRootNodeType());
            }
            if (!CapabilityTypes.ContainsKey(ToscaDefaults.ToscaCapabilitiesRoot))
            {
                AddCapabilityType(ToscaDefaults.ToscaCapabilitiesRoot, ToscaDefaults.GetRootCapabilityType());
            }
            if (!CapabilityTypes.ContainsKey(ToscaDefaults.ToscaCapabilitiesNode))
            {
                AddCapabilityType(ToscaDefaults.ToscaCapabilitiesNode, ToscaDefaults.GetNodeCapabilityType());
            }
            foreach (var keyValuePair in NodeTypes.Where(n => n.Value.IsRoot() && n.Key != ToscaDefaults.ToscaNodesRoot))
            {
                keyValuePair.Value.DerivedFrom = ToscaDefaults.ToscaNodesRoot;
            }
        }

        /// <summary>
        /// Implements Validate method from <see cref="IValidatableObject"/> interface
        /// </summary>
        /// <param name="validationContext">Context the validation runs in</param>
        /// <returns>List of validation results if any</returns>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            //if ( GetEntryPointServiceTemplate() ToscaMetadata.EntryDefinitions)
            var validationResults = new List<ValidationResult>();
            foreach (var nodeTypeKeyValue in NodeTypes)
            {
                foreach (var requirementKeyValue in nodeTypeKeyValue.Value.Requirements.SelectMany(r=>r).ToArray())
                {
                    if (!NodeTypes.ContainsKey(requirementKeyValue.Value.Node))
                    {
                        validationResults.Add(CreateRequirementValidationResult(requirementKeyValue, nodeTypeKeyValue));
                    }
                }
                foreach (var capabilityKeyValue in nodeTypeKeyValue.Value.Capabilities)
                {
                    if (!CapabilityTypes.ContainsKey(capabilityKeyValue.Value.Type))
                    {
                        validationResults.Add(CreateCapabilityTypeValidationResult(nodeTypeKeyValue.Key, capabilityKeyValue.Value.Type, capabilityKeyValue.Key));
                    }
                }
            }
            return validationResults;
        }

        /// <summary>
        /// Tries to validate the instance and populates list of validation results if any
        /// </summary>
        /// <param name="validationResults">List of validation results</param>
        /// <returns>True if valid, false otherwise</returns>
        public bool TryValidate(out List<ValidationResult> validationResults)
        {
            var cloudServiceValidator = new Bootstrapper().GetToscaCloudServiceValidator();
            return cloudServiceValidator.TryValidateRecursively(this, out validationResults);
        }

        private ValidationResult CreateCapabilityTypeValidationResult(string nodeTypeName, string capabilityType, string capability)
        {
            return new ValidationResult(
                string.Format("Capability type '{0}' attached to node '{1}' as capability '{2}' not found.",
                     capabilityType,
                     nodeTypeName,
                     capability)); 
        }

        /// <summary>
        /// Traverses node types starting from 'tosca.nodes.Root', then to its derived node types and so on 
        /// </summary>
        /// <param name="action">Action to be executed on each node type when visiting a node type</param>
        public void TraverseNodeTypesInheritance(Action<string, ToscaNodeType> action)
        {
            var serviceArchiveWalker = new ToscaNodeTypeInheritanceWalker(this, action);
            serviceArchiveWalker.Walk();
        }

        /// <summary>
        /// Traverses node types starting from nodeTypeNameToStart, then to its requirements nodes types and so on
        /// </summary>
        /// <param name="nodeTypeNameToStart">Name of a node type to start the traversal</param>
        /// <param name="action">Action to be executed on each node type when visiting a node type</param>
        public void TraverseNodeTypesByRequirements(string nodeTypeNameToStart, Action<string, ToscaNodeType> action)
        {
            if (!NodeTypes.ContainsKey(nodeTypeNameToStart))
            {
                throw new ToscaNodeTypeNotFoundException(string.Format("Node type '{0}' not found", nodeTypeNameToStart));
            }
            var serviceArchiveWalker = new ToscaNodeTypeRequirementsWalker(this, action);
            serviceArchiveWalker.Walk(nodeTypeNameToStart);
        }

        private static ValidationResult CreateRequirementValidationResult(KeyValuePair<string, ToscaRequirement> requirementKeyValue, KeyValuePair<string, ToscaNodeType> nodeTypeKeyValue)
        {
            return new ValidationResult(string.Format("Node '{0}' of requirement '{1}' on node type '{2}' not found.", 
                requirementKeyValue.Value.Node, 
                requirementKeyValue.Key, 
                nodeTypeKeyValue.Key));
        }
    }
}