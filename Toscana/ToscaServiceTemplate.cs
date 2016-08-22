using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using Toscana.Common;
using Toscana.Engine;

namespace Toscana
{
    /// <summary>
    /// A TOSCA Service Template (YAML) document contains element definitions of building blocks for cloud application, 
    /// or complete models of cloud applications. This section describes the top-level structural 
    /// elements (TOSCA keynames) along with their grammars, which are allowed to appear in a TOSCA Service Template document.
    /// </summary>
    public class ToscaServiceTemplate
    {
        /// <summary>
        /// Initializes an instance of Service Template
        /// </summary>
        public ToscaServiceTemplate()
        {
            CapabilityTypes = new Dictionary<string, ToscaCapabilityType>();
            NodeTypes = new Dictionary<string, ToscaNodeType>();
            Imports = new List<Dictionary<string, ToscaImport>>();
            Metadata = new ToscaServiceTemplateMetadata();
            RelationshipTypes = new Dictionary<string, ToscaRelationshipType>();
            TopologyTemplate = new ToscaTopologyTemplate();
        }

        /// <summary>
        /// Defines the version of the TOSCA Simple Profile specification the template (grammar) complies with.
        /// </summary>
        [Required(ErrorMessage = "tosca_definitions_version is required on tosca definition")]
        [RestrictedValues(new[] { "tosca_simple_yaml_1_0" }, "tosca_definitions_version shall be tosca_simple_yaml_1_0")]
        public string ToscaDefinitionsVersion { get; set; }

        /// <summary>
        /// Defines a section used to declare additional metadata information.  Domain-specific TOSCA profile specifications may define keynames that are required for their implementations.
        /// </summary>
        public ToscaServiceTemplateMetadata Metadata { get; set; }

        /// <summary>
        /// Declares a description for this Service Template and its contents.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Declares optional DSL-specific definitions and conventions.  For example, in YAML, this allows defining reusable YAML macros (i.e., YAML alias anchors) for use throughout the TOSCA Service Template.
        /// </summary>
        public object DslDefinitions { get; set; }

        /// <summary>
        /// Declares the list of external repositories which contain artifacts that are referenced in the service template 
        /// along with their addresses and necessary credential information used to connect to them in order to retrieve the artifacts.
        /// </summary>
        public Dictionary<string, ToscaRepository> Repositories { get; set; }

        /// <summary>
        /// Declares import statements external TOSCA Definitions documents. 
        /// For example, these may be file location or URIs relative to the service template file within the same TOSCA CSAR file.
        /// </summary>
        public List<Dictionary<string, ToscaImport>> Imports { get; set; }

        /// <summary>
        /// This section contains an optional list of artifact type definitions for use in the service template
        /// </summary>
        public Dictionary<string, ToscaArtifactType> ArtifactTypes { get; set; }

        /// <summary>
        /// This section contains an optional list of capability type definitions for use in the service template
        /// </summary>
        public Dictionary<string, ToscaCapabilityType> CapabilityTypes { get; set; }

        /// <summary>
        /// Defines the topology template of an application or service, consisting of node templates that represent 
        /// the application’s or service’s components, as well as relationship templates representing relations between the components.
        /// </summary>
        public ToscaTopologyTemplate TopologyTemplate { get; set; }

        /// <summary>
        /// This section contains a set of node type definitions for use in the service template.
        /// </summary>
        public Dictionary<string, ToscaNodeType> NodeTypes { get; set; }

        /// <summary>
        /// This section contains a set of relationship type definitions for use in the service template.
        /// </summary>
        public Dictionary<string, ToscaRelationshipType> RelationshipTypes { get; set; }

        /// <summary>
        /// Parses stream of TOSCA YAML file into an instance of ToscaServiceTemplate class
        /// </summary>
        /// <param name="toscaAsString">TOSCA YAML content</param>
        /// <returns>Valid instance of ToscaServiceTemplate</returns>
        public static ToscaServiceTemplate Parse(string toscaAsString)
        {
            var toscaSimpleProfileParser = new Bootstrapper().GetToscaServiceTemplateParser();
            return toscaSimpleProfileParser.Parse(toscaAsString);
        }

        /// <summary>
        /// Parses stream of TOSCA YAML file into an instance of ToscaServiceTemplate class
        /// </summary>
        /// <param name="stream">Stream of TOSCA YAML file</param>
        /// <returns>Valid instance of ToscaServiceTemplate</returns>
        public static ToscaServiceTemplate Parse(Stream stream)
        {
            var toscaServiceTemplateParser = new Bootstrapper().GetToscaServiceTemplateParser();
            return toscaServiceTemplateParser.Parse(stream);
        }
    }
}