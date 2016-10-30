using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using Toscana.Common;
using Toscana.Engine;
using Toscana.Exceptions;

namespace Toscana
{
    /// <summary>
    ///     Represents Tosca Cloud Service Archive (CSAR), which is an archive containing
    ///     TOSCA Simple Profile definitions along with all accompanying artifacts (e.g. scripts, binaries, configuration
    ///     files)
    ///     A CSAR zip file is required to contain a TOSCA-Metadata directory,
    ///     which in turn contains the TOSCA.meta metadata file that provides entry information
    ///     for a TOSCA orchestrator processing the CSAR file.
    ///     The CSAR file may contain other directories with arbitrary names and contents.
    /// </summary>
    public class ToscaCloudServiceArchive : IValidatableObject, IToscaMetadata
    {
        private const string ArchitectureDescription = @"The Operating System (OS) architecture.
 
Examples of valid values include:
x86_32, x86_64, etc.";
        private const string OperatingSystemTypeDescription = @"The Operating System (OS) type.
 
Examples of valid values include:
linux, aix, mac, windows, etc.";
        private const string OperationSystemDistributionDescription = @"The Operating System (OS) distribution.
 
Examples of valid values for an “type” of “Linux” would include:  debian, fedora, rhel and ubuntu.";
        private const string OperatingSystemVersionDescription = "The Operating System version.";

        #region IValidatableObject implementation

        /// <summary>
        ///     Implements Validate method from <see cref="IValidatableObject" /> interface
        /// </summary>
        /// <param name="validationContext">Context the validation runs in</param>
        /// <returns>List of validation results if any</returns>
        IEnumerable<ValidationResult> IValidatableObject.Validate(ValidationContext validationContext)
        {
            var validationResults = new List<ValidationResult>();
            foreach (var nodeTypeKeyValue in NodeTypes)
            {
                nodeTypeKeyValue.Value.SetDerivedFromToRoot(nodeTypeKeyValue.Key);

                foreach (var requirementKeyValue in nodeTypeKeyValue.Value.Requirements.SelectMany(r => r).ToArray())
                {
                    if (!string.IsNullOrEmpty(requirementKeyValue.Value.Node) &&
                        !NodeTypes.ContainsKey(requirementKeyValue.Value.Node))
                    {
                        validationResults.Add(CreateRequirementValidationResult(requirementKeyValue, nodeTypeKeyValue));
                    }
                }

                foreach (var capabilityKeyValue in nodeTypeKeyValue.Value.Capabilities)
                {
                    if (!CapabilityTypes.ContainsKey(capabilityKeyValue.Value.Type))
                    {
                        validationResults.Add(CreateCapabilityTypeValidationResult(nodeTypeKeyValue.Key,
                            capabilityKeyValue.Value.Type, capabilityKeyValue.Key));
                    }
                }

                foreach (var complexDataTypeKeyValue in DataTypes)
                {
                    foreach (var basicDatatypeKeyValue in complexDataTypeKeyValue.Value.Properties)
                    {
                        var basicType = basicDatatypeKeyValue.Value.Type;
                        if (!DataTypes.ContainsKey(basicType))
                        {
                            validationResults.Add(new ValidationResult(
                                string.Format("Data type '{0}' specified as part of data type '{1}' not found.",
                                    basicType,
                                    complexDataTypeKeyValue.Key)));
                        }
                    }
                }

                var circularDependencyValidationResults = nodeTypeKeyValue.Value.ValidateCircularDependency().ToList();
                if (circularDependencyValidationResults.Any())
                {
                    return validationResults.Concat(circularDependencyValidationResults);
                }
            }

            var importCircularValidationResults = ValidateImportsCircularDependency();
            if ( importCircularValidationResults.Any())
                    return validationResults.Concat(importCircularValidationResults);

            if (!validationResults.Any())
            {
                var requirementsGraph = new ToscaNodeTypeRequirementsGraph(this);
                if (requirementsGraph.ContainsCyclicLoop())
                {
                    validationResults.Add(new ValidationResult("Circular dependency detected by requirements on node type"));
                }
            }
            return validationResults;
        }

        private List<ValidationResult> ValidateImportsCircularDependency()
        {
            var imports = new HashSet<string>();
            return ValidationImportsRecuresively(imports, GetEntryPointServiceTemplate());
        }

        private List<ValidationResult> ValidationImportsRecuresively(HashSet<string> imports, ToscaServiceTemplate serviceTemplate)
        {
            foreach (var import in serviceTemplate.GetImports())
            {
                if (imports.Contains(import.File))
                {
                    return new List<ValidationResult>
                    {
                        new ValidationResult(string.Format("Circular dependency detected on import '{0}'", import.File))
                    };
                }
                imports.Add(import.File);
                return ValidationImportsRecuresively(imports, ToscaServiceTemplates[import.File]);
            }
            return new List<ValidationResult>();
        }

        #endregion

        /// <summary>
        ///     Saves Cloud Service Archive to a stream
        /// </summary>
        /// <param name="stream">Stream to save</param>
        public void Save(Stream stream)
        {
            var cloudServiceArchiveSaver = Bootstrapper.Current.GetToscaCloudServiceArchiveSaver();
            cloudServiceArchiveSaver.Save(this, stream);
        }

        /// <summary>
        /// Saves Cloud Service Archive to file 
        /// </summary>
        /// <param name="filePath">Path to file to save</param>
        public void Save(string filePath)
        {
            var toscaCloudServiceArchiveSaver = Bootstrapper.Current.GetToscaCloudServiceArchiveSaver();
            toscaCloudServiceArchiveSaver.Save(this, filePath);
        }

        #region Private fields

        private readonly Dictionary<string, ToscaNodeType> nodeTypes;
        private readonly Dictionary<string, ToscaServiceTemplate> toscaServiceTemplates;
        private readonly Dictionary<string, byte[]> fileContents;
        private readonly Dictionary<string, ToscaCapabilityType> capabilityTypes;
        private readonly Dictionary<string, ToscaArtifactType> artifactTypes;
        private readonly Dictionary<string, ToscaRelationshipType> relationshipTypes;
        private readonly Dictionary<string, ToscaDataType> dataTypes;
        private readonly ToscaMetadata toscaMetadata;

        #endregion

        #region Public ctors

        /// <summary>
        ///     Instantiate an instance of <see cref="ToscaCloudServiceArchive" /> from a ToscaMetadata and an optional list of
        ///     archive
        ///     entries of its content.
        /// </summary>
        /// <param name="toscaMetadata">An instance of Tosca Metadata</param>
        public ToscaCloudServiceArchive(ToscaMetadata toscaMetadata)
        {
            this.toscaMetadata = toscaMetadata;
            toscaServiceTemplates = new Dictionary<string, ToscaServiceTemplate>();
            nodeTypes = new Dictionary<string, ToscaNodeType>();
            capabilityTypes = new Dictionary<string, ToscaCapabilityType>();
            artifactTypes = new Dictionary<string, ToscaArtifactType>();
            relationshipTypes = new Dictionary<string, ToscaRelationshipType>();
            fileContents = new Dictionary<string, byte[]>(new PathEqualityComparer());
            dataTypes = new Dictionary<string, ToscaDataType>();
            FillDefaults();
        }

        #endregion

        #region Public properties

        /// <summary>
        ///     TOSCA Metadata
        /// </summary>
        internal ToscaMetadata ToscaMetadata
        {
            get { return toscaMetadata; }
        }

        /// <summary>
        /// </summary>
        public IReadOnlyDictionary<string, ToscaServiceTemplate> ToscaServiceTemplates
        {
            get { return toscaServiceTemplates; }
        }

        /// <summary>
        ///     An aggregated dictionary of the node types from all Tosca Service Templates
        /// </summary>
        public IReadOnlyDictionary<string, ToscaNodeType> NodeTypes
        {
            get { return nodeTypes; }
        }

        /// <summary>
        ///     Returns capability from all the Service Templates
        /// </summary>
        public IReadOnlyDictionary<string, ToscaCapabilityType> CapabilityTypes
        {
            get { return capabilityTypes; }
        }

        /// <summary>
        ///     Returns artifact types from all the Service Templates
        /// </summary>
        public IReadOnlyDictionary<string, ToscaArtifactType> ArtifactTypes
        {
            get { return artifactTypes; }
        }

        /// <summary>
        ///     Returns relationship types from all the Service Templates
        /// </summary>
        public IReadOnlyDictionary<string, ToscaRelationshipType> RelationshipTypes
        {
            get { return relationshipTypes; }
        }

        /// <summary>
        ///     Returns artifacts as byte array
        /// </summary>
        public IReadOnlyDictionary<string, byte[]> Artifacts
        {
            get { return fileContents; }
        }

        /// <summary>
        ///     Returns data types from all the Service Templates
        /// </summary>
        public IReadOnlyDictionary<string, ToscaDataType> DataTypes
        {
            get { return dataTypes; }
        }

        #endregion

        #region Public methods

        /// <summary>
        ///     Returns ToscaServiceTemplate that is pointed by Entry-Definitions from TOSCA.meta file
        /// </summary>
        /// <returns></returns>
        public ToscaServiceTemplate GetEntryPointServiceTemplate()
        {
            return toscaServiceTemplates[ToscaMetadata.EntryDefinitions];
        }

        /// <summary>
        ///     Loads Cloud Service Archive (CSAR) file and all its dependencies
        /// </summary>
        /// <param name="archiveFilePath">Path to Cloud Service Archive (CSAR) zip file</param>
        /// <param name="alternativePath">Path for dependencies lookup outside the archive</param>
        /// <exception cref="Toscana.Exceptions.ToscaCloudServiceArchiveFileNotFoundException">Thrown when CSAR file not found.</exception>
        /// <exception cref="ToscaMetadataFileNotFoundException">Thrown when TOSCA.meta file not found in the archive.</exception>
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
        /// <exception cref="ToscaMetadataFileNotFoundException">Thrown when TOSCA.meta file not found in the archive.</exception>
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

        /// <summary>
        ///     Adds a ToscaServiceTemplate
        /// </summary>
        /// <exception cref="ToscaArtifactNotFoundException">Thrown when artifact not found in the Cloud Service Archive.</exception>
        /// <param name="toscaServiceTemplateName">Service template name</param>
        /// <param name="toscaServiceTemplate">An instance of ToscaServiceTemplate</param>
        /// <exception cref="ToscaServiceTemplateAlreadyExistsException">
        ///     Service Template was already added to the Cloud Service
        ///     Archive.
        /// </exception>
        public void AddToscaServiceTemplate(string toscaServiceTemplateName, ToscaServiceTemplate toscaServiceTemplate)
        {
            if (toscaServiceTemplates.ContainsKey(toscaServiceTemplateName))
            {
                throw new ToscaServiceTemplateAlreadyExistsException(
                    string.Format("Service Template '{0}' already exists", toscaServiceTemplateName));
            }
            toscaServiceTemplates.Add(toscaServiceTemplateName, toscaServiceTemplate);
            foreach (var toscaNodeType in toscaServiceTemplate.NodeTypes)
            {
                AddNodeType(toscaNodeType.Key, toscaNodeType.Value);
            }
            foreach (var capabilityType in toscaServiceTemplate.CapabilityTypes)
            {
                AddCapabilityType(capabilityType.Key, capabilityType.Value);
            }
            foreach (var dataTypeKeyValue in toscaServiceTemplate.DataTypes)
            {
                AddDataType(dataTypeKeyValue.Key, dataTypeKeyValue.Value);
            }
        }

        private void AddDataType(string dataTypeName, ToscaDataType dataType)
        {
            dataTypes.Add(dataTypeName, dataType);
            dataType.SetCloudServiceArchive(this);
            dataType.SetDerivedFromToRoot(dataTypeName);
        }

        /// <summary>
        ///     Returns ToscaNodeTypes from the Entry TOSCA YAML file that are not used in DerivedFrom of other node types,
        ///     e.g. are leaves in node types inheritance.
        /// </summary>
        /// <returns></returns>
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
        ///     Gets file content as byte array
        /// </summary>
        /// <param name="fileName">File name to return content of</param>
        /// <returns>File content as byte array</returns>
        /// <exception cref="ToscaArtifactNotFoundException">Thrown when artifact with fileName is not found.</exception>
        public byte[] GetArtifactBytes(string fileName)
        {
            byte[] content;
            if (!fileContents.TryGetValue(fileName, out content))
            {
                throw new ToscaArtifactNotFoundException(
                    string.Format("Artifact '{0}' not found in Cloud Service Archive.",
                        fileName));
            }
            return content;
        }

        /// <summary>
        ///     Determines whether <see cref="ToscaCloudServiceArchive" /> contains a file
        /// </summary>
        /// <param name="artifactPath">Artifact path to check</param>
        /// <returns>True is contains, false otherwise</returns>
        public bool ContainsArtifact(string artifactPath)
        {
            return fileContents.ContainsKey(artifactPath);
        }

        /// <summary>
        ///     Adds an artifact to Cloud Service Archive
        /// </summary>
        /// <param name="artifactPath">Relative path to file path</param>
        /// <param name="bytes">Byte array of the artifact</param>
        public void AddArtifact(string artifactPath, byte[] bytes)
        {
            fileContents[artifactPath] = bytes;
        }

        /// <summary>
        ///     Tries to validate the instance and populates list of validation results if any
        /// </summary>
        /// <param name="validationResults">List of validation results</param>
        /// <returns>True if valid, false otherwise</returns>
        public bool TryValidate(out List<ValidationResult> validationResults)
        {
            var cloudServiceValidator = Bootstrapper.Current.GetToscaCloudServiceValidator();
            return cloudServiceValidator.ValidateCloudServiceArchive(this, out validationResults);
        }

        /// <summary>
        ///     Traverses node types starting from 'tosca.nodes.Root', then to its derived node types and so on
        /// </summary>
        /// <param name="action">Action to be executed on each node type when visiting a node type</param>
        public void TraverseNodeTypesInheritance(Action<string, ToscaNodeType> action)
        {
            var serviceArchiveWalker = new ToscaNodeTypeInheritanceWalker(this, action);
            serviceArchiveWalker.Walk();
        }

        /// <summary>
        ///     Traverses node types starting from nodeTypeNameToStart, then to its requirements nodes types and so on
        /// </summary>
        /// <param name="nodeTypeNameToStart">Name of a node type to start the traversal</param>
        /// <param name="action">Action to be executed on each node type when visiting a node type</param>
        /// <exception cref="ToscaNodeTypeNotFoundException">Thrown when nodeTypeNameToStart is not found in NodeTypes dictionary.</exception>
        public void TraverseNodeTypesByRequirements(string nodeTypeNameToStart, Action<string, ToscaNodeType> action)
        {
            if (!NodeTypes.ContainsKey(nodeTypeNameToStart))
            {
                throw new ToscaNodeTypeNotFoundException(string.Format("Node type '{0}' not found", nodeTypeNameToStart));
            }
            var serviceArchiveWalker = new ToscaNodeTypeRequirementsGraph(this);
            serviceArchiveWalker.Walk(nodeTypeNameToStart, action);
        }

        #endregion

        #region Private methods

        private static IToscaCloudServiceArchiveLoader GetToscaCloudServiceArchiveLoader()
        {
            return Bootstrapper.Current.GetToscaCloudServiceArchiveLoader();
        }

        private ValidationResult CreateCapabilityTypeValidationResult(string nodeTypeName, string capabilityType,
            string capability)
        {
            return new ValidationResult(
                string.Format("Capability type '{0}' attached to node '{1}' as capability '{2}' not found.",
                    capabilityType,
                    nodeTypeName,
                    capability));
        }

        private static ValidationResult CreateRequirementValidationResult(
            KeyValuePair<string, ToscaRequirement> requirementKeyValue,
            KeyValuePair<string, ToscaNodeType> nodeTypeKeyValue)
        {
            return new ValidationResult(string.Format("Node '{0}' of requirement '{1}' on node type '{2}' not found.",
                requirementKeyValue.Value.Node,
                requirementKeyValue.Key,
                nodeTypeKeyValue.Key));
        }

        private void AddCapabilityType(string capabilityTypeName, ToscaCapabilityType capabilityType)
        {
            capabilityTypes.Add(capabilityTypeName, capabilityType);
            capabilityType.SetCloudServiceArchive(this);
            capabilityType.SetDerivedFromToRoot(capabilityTypeName);
        }

        /// <summary>
        ///     Adds a node type with its name
        /// </summary>
        /// <param name="nodeTypeName">Node type name</param>
        /// <param name="nodeType">An instance of node type to add</param>
        /// <exception cref="ToscaArtifactNotFoundException"></exception>
        private void AddNodeType(string nodeTypeName, ToscaNodeType nodeType)
        {
            nodeTypes.Add(nodeTypeName, nodeType);
            nodeType.SetCloudServiceArchive(this);
            nodeType.SetDerivedFromToRoot(nodeTypeName);
        }

        /// <summary>
        ///     If needed adds built-in node types and capabilities
        /// </summary>
        private void FillDefaults()
        {
            AddNodeType(ToscaDefaults.ToscaNodesRoot, ToscaDefaults.GetRootNodeType());
            AddNodeType("tosca.nodes.BlockStorage", CreateBlockStorageNodeType());
            AddNodeType("tosca.nodes.Compute", CreateComputeNodeType());
            AddCapabilityType("tosca.capabilities.Container", CreateContainerCapabilityType());
            var endpointAdminCapabilityType = new ToscaCapabilityType {DerivedFrom = "tosca.capabilities.Endpoint" };
            endpointAdminCapabilityType.Properties.Add("secure", new ToscaProperty { Type = "boolean", Default = true});
            AddCapabilityType("tosca.capabilities.Endpoint.Admin", endpointAdminCapabilityType);
            AddCapabilityType("tosca.capabilities.Endpoint", CreateEndpointCapabilityType());
            AddCapabilityType("tosca.capabilities.Attachment", new ToscaCapabilityType { DerivedFrom = "tosca.capabilities.Root" });
            AddCapabilityType(ToscaDefaults.ToscaCapabilitiesRoot, ToscaDefaults.GetRootCapabilityType());
            AddCapabilityType(ToscaDefaults.ToscaCapabilitiesNode, ToscaDefaults.GetNodeCapabilityType());
            AddCapabilityType("tosca.capabilities.OperatingSystem", CreateOperatingSystemCapabilityType());
            AddCapabilityType("tosca.capabilities.Scalable", CreateScalableCapabilityType());
            AddCapabilityType("tosca.capabilities.network.Bindable", CreateBindableCapabilityType());
            AddDataType(ToscaDefaults.ToscaDataTypeRoot, ToscaDefaults.GetRootDataType());
            AddDataType("string", new ToscaDataType {DerivedFrom = ToscaDefaults.ToscaDataTypeRoot});
            AddDataType("integer", new ToscaDataType {DerivedFrom = ToscaDefaults.ToscaDataTypeRoot});
            AddDataType("float", new ToscaDataType {DerivedFrom = ToscaDefaults.ToscaDataTypeRoot});
            AddDataType("boolean", new ToscaDataType {DerivedFrom = ToscaDefaults.ToscaDataTypeRoot});
            AddDataType("null", new ToscaDataType {DerivedFrom = ToscaDefaults.ToscaDataTypeRoot});
        }

        private static ToscaCapabilityType CreateBindableCapabilityType()
        {
            return new ToscaCapabilityType { DerivedFrom = "tosca.capabilities.Node" };
        }

        private static ToscaCapabilityType CreateScalableCapabilityType()
        {
            var scalableCapabilityType = new ToscaCapabilityType() { DerivedFrom = "tosca.capabilities.Root" };
            scalableCapabilityType.Properties.Add("min_instances", new ToscaProperty { Type = "integer", Default = 1});
            scalableCapabilityType.Properties.Add("max_instances", new ToscaProperty { Type = "integer", Default = 1});
            scalableCapabilityType.Properties.Add("default_instances", new ToscaProperty { Type = "integer" });
            return scalableCapabilityType;
        }

        private static ToscaCapabilityType CreateOperatingSystemCapabilityType()
        {
            var operatingSystemCapabilityType = new ToscaCapabilityType { DerivedFrom = "tosca.capabilities.Root" };
            operatingSystemCapabilityType.Properties.Add("architecture", new ToscaProperty { Type = "string", Required = false, Description = ToscaCloudServiceArchive.ArchitectureDescription });
            operatingSystemCapabilityType.Properties.Add("type", new ToscaProperty { Type = "string", Required = false, Description = ToscaCloudServiceArchive.OperatingSystemTypeDescription });
            operatingSystemCapabilityType.Properties.Add("distribution", new ToscaProperty { Type = "string", Required = false, Description = ToscaCloudServiceArchive.OperationSystemDistributionDescription });
            operatingSystemCapabilityType.Properties.Add("version", new ToscaProperty { Type = "version", Required = false, Description = ToscaCloudServiceArchive.OperatingSystemVersionDescription });
            return operatingSystemCapabilityType;
        }

        private static ToscaCapabilityType CreateEndpointCapabilityType()
        {
            var endpointCapabilityType = new ToscaCapabilityType { DerivedFrom = "tosca.capabilities.Root" };
            endpointCapabilityType.Properties.Add("protocol", new ToscaProperty { Type = "string", Default = "tcp"});
            endpointCapabilityType.Properties.Add("port", new ToscaProperty { Type = "PortDef", Required = false });
            endpointCapabilityType.Properties.Add("secure", new ToscaProperty { Type = "boolean", Default = false });
            endpointCapabilityType.Properties.Add("url_path", new ToscaProperty { Type = "string", Required = false });
            endpointCapabilityType.Properties.Add("port_name", new ToscaProperty { Type = "string", Required = false });
            endpointCapabilityType.Properties.Add("network_name", new ToscaProperty { Type = "string", Required = false, Default = "PRIVATE"});
            var initiatorProperty = new ToscaProperty { Type = "string", Default = "source"};
            initiatorProperty.AddConstraint("valid_values", new [] {"source", "target", "peer"});
            endpointCapabilityType.Properties.Add("initiator", initiatorProperty);
            var portsProperty = new ToscaProperty { Type = "map", Required = false, EntrySchema = new ToscaPropertyEntrySchema { Type = "PortSpec"} };
            portsProperty.AddConstraint("min_length", 1);
            endpointCapabilityType.Properties.Add("ports", portsProperty);
            endpointCapabilityType.Attributes.Add("ip_address", new ToscaAttribute { Type = "string"});
            return endpointCapabilityType;
        }

        private static ToscaCapabilityType CreateContainerCapabilityType()
        {
            var containerCapabilityType = new ToscaCapabilityType { DerivedFrom = "tosca.capabilities.Root" };
            var numCpusProperty = new ToscaProperty { Type = "integer", Required = false};
            numCpusProperty.AddConstraint("greater_or_equal", "1");
            containerCapabilityType.Properties.Add("num_cpus", numCpusProperty);
            var cpuFrequencyProperty = new ToscaProperty {Type = "scalar-unit.frequency", Required = false};
            cpuFrequencyProperty.AddConstraint("greater_or_equal", "0.1 GHz");
            containerCapabilityType.Properties.Add("cpu_frequency", cpuFrequencyProperty);
            var diskSizeProperty = new ToscaProperty { Type = "scalar-unit.size", Required = false };
            diskSizeProperty.AddConstraint("greater_or_equal", "O MB");
            containerCapabilityType.Properties.Add("disk_size", diskSizeProperty);
            var memSizeProperty = new ToscaProperty { Type = "scalar-unit.size", Required = false};
            memSizeProperty.AddConstraint("greater_or_equal", "0 MB");
            containerCapabilityType.Properties.Add("mem_size", memSizeProperty);
            return containerCapabilityType;
        }

        private static ToscaNodeType CreateBlockStorageNodeType()
        {
            var blockStorageNodeType = new ToscaNodeType {DerivedFrom = "tosca.nodes.Root"};
            var sizeProperty = new ToscaProperty {Type = "scalar-unit.size"};
            sizeProperty.AddConstraint("greater_or_equal", "1 MB");
            blockStorageNodeType.Properties.Add("size", sizeProperty);
            blockStorageNodeType.Properties.Add("volume_id", new ToscaProperty {Type = "string", Required = false});
            blockStorageNodeType.Properties.Add("snapshot_id", new ToscaProperty {Type = "string", Required = false});
            blockStorageNodeType.Capabilities.Add("attachment", new ToscaCapability {Type = "tosca.capabilities.Attachment"});
            return blockStorageNodeType;
        }

        private static ToscaNodeType CreateComputeNodeType()
        {
            var computeNodeType = new ToscaNodeType {DerivedFrom = ToscaDefaults.ToscaNodesRoot};
            computeNodeType.Attributes.Add("private_address", new ToscaAttribute {Type = "string"});
            computeNodeType.Attributes.Add("public_address", new ToscaAttribute {Type = "string"});
            computeNodeType.Attributes.Add("networks", new ToscaAttribute
            {
                Type = "map",
                EntrySchema = new ToscaAttribute
                {
                    Type = "tosca.datatypes.network.NetworkInfo"
                }
            });
            computeNodeType.Attributes.Add("ports", new ToscaAttribute
            {
                Type = "map",
                EntrySchema = new ToscaAttribute
                {
                    Type = "tosca.datatypes.network.PortInfo"
                }
            });
            computeNodeType.AddRequirement("local_storage",
                new ToscaRequirement()
                {
                    Capability = "tosca.capabilities.Attachment",
                    Node = "tosca.nodes.BlockStorage",
                    Relationship = "tosca.relationships.AttachesTo",
                    Occurrences = new object[] {0, "UNBOUNDED"}
                });
            computeNodeType.Capabilities.Add("host",
                new ToscaCapability
                {
                    Type = "tosca.capabilities.Container",
                    ValidSourceTypes = new[] {"tosca.nodes.SoftwareComponent"}
                });
            computeNodeType.Capabilities.Add("endpoint", new ToscaCapability {Type = "tosca.capabilities.Endpoint.Admin"});
            computeNodeType.Capabilities.Add("os", new ToscaCapability {Type = "tosca.capabilities.OperatingSystem"});
            computeNodeType.Capabilities.Add("scalable", new ToscaCapability {Type = "tosca.capabilities.Scalable"});
            computeNodeType.Capabilities.Add("binding", new ToscaCapability {Type = "tosca.capabilities.network.Bindable"});
            return computeNodeType;
        }

        #endregion

        /// <summary>
        /// Specifies TOSCA.meta file version
        /// </summary>
        public Version ToscaMetaFileVersion
        {
            get { return ToscaMetadata.ToscaMetaFileVersion; }
        }

        /// <summary>
        /// Denotes the verison of CSAR
        /// Due to the simplified structure of the CSAR file and TOSCA.meta file compared to TOSCA 1.0, 
        /// the CSAR-Version keyword listed in block_0 of the meta-file is required to denote version 1.1.
        /// </summary>
        public Version CsarVersion
        {
            get { return ToscaMetadata.CsarVersion; }
        }

        /// <summary>
        /// Specifies who created the CSAR  
        /// </summary>
        public string CreatedBy
        {
            get { return ToscaMetadata.CreatedBy; }
        }

        /// <summary>
        /// Entry-Definitions keyword pointing to a valid TOSCA definitions YAML file that a TOSCA 
        /// orchestrator should use as entry for parsing the contents of the overall CSAR file.
        /// </summary>
        public string EntryDefinitions
        {
            get { return ToscaMetadata.EntryDefinitions; }
        }
    }
}