using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Toscana.Domain
{
    public class Tosca
    {
        [Required(ErrorMessage = "tosca_definitions_version is required on tosca definition")]
        public string ToscaDefinitionsVersion { get; set; }

        public Dictionary<string, CapabilityType> CapabilityTypes { get; set; }
        public string Description { get; set; }
        public TopologyTemplate TopologyTemplate { get; set; }
        public Dictionary<string, NodeType> NodeTypes { get; set; }
        public List<Dictionary<string, ToscaImport>> Imports { get; set; }
    }
}