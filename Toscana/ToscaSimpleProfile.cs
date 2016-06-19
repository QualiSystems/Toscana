using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Toscana.Common;
using Toscana.Engine;

namespace Toscana
{
    public class ToscaSimpleProfile
    {
        public ToscaSimpleProfile()
        {
            CapabilityTypes = new Dictionary<string, ToscaCapabilityType>();
            NodeTypes = new Dictionary<string, ToscaNodeType>();
            Imports = new List<Dictionary<string, ToscaImport>>();
        }

        [Required(ErrorMessage = "tosca_definitions_version is required on tosca definition")]
        [RestrictedValues(new[] { "tosca_simple_yaml_1_0" }, "tosca_definitions_version shall be tosca_simple_yaml_1_0")]
        public string ToscaDefinitionsVersion { get; set; }

        public Dictionary<string, ToscaCapabilityType> CapabilityTypes { get; set; }
        public string Description { get; set; }
        public ToscaTopologyTemplate TopologyTemplate { get; set; }
        public Dictionary<string, ToscaNodeType> NodeTypes { get; set; }
        public List<Dictionary<string, ToscaImport>> Imports { get; set; }

        public static ToscaSimpleProfile Parse(string toscaAsString)
        {
            var toscaSimpleProfileParser = Bootstrapper.GetToscaSimpleProfileParser();
            return toscaSimpleProfileParser.Parse(toscaAsString);
        }
    }
}