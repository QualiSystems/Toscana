using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using Toscana.Common;
using Toscana.Engine;

namespace Toscana
{
    public class ToscaServiceTemplate
    {
        public ToscaServiceTemplate()
        {
            CapabilityTypes = new Dictionary<string, ToscaCapabilityType>();
            NodeTypes = new Dictionary<string, ToscaNodeType>();
            Imports = new List<Dictionary<string, ToscaImport>>();
            Metadata = new ToscaServiceTemplateMetadata();
            RelationshipTypes = new Dictionary<string, ToscaRelationshipType>();
            TopologyTemplate = new ToscaTopologyTemplate();
        }

        [Required(ErrorMessage = "tosca_definitions_version is required on tosca definition")]
        [RestrictedValues(new[] { "tosca_simple_yaml_1_0" }, "tosca_definitions_version shall be tosca_simple_yaml_1_0")]
        public string ToscaDefinitionsVersion { get; set; }

        public Dictionary<string, ToscaCapabilityType> CapabilityTypes { get; set; }
        public string Description { get; set; }
        public ToscaTopologyTemplate TopologyTemplate { get; set; }
        public Dictionary<string, ToscaNodeType> NodeTypes { get; set; }
        public List<Dictionary<string, ToscaImport>> Imports { get; set; }
        public ToscaServiceTemplateMetadata Metadata { get; set; }
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