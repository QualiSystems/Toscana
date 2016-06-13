using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

// ReSharper disable All

namespace Toscana.Domain
{
    public class NodeTemplate
    {
        [Required(ErrorMessage = "type is required on node_template")]
        public string Type { get; set; }

        public string Description { get; set; }

        public string[] Directives { get; set; }

        public Dictionary<string, object> Properties { get; set; }
        public Dictionary<string, AttributeAssignment> Attributes { get; set; }
        public List<Dictionary<string, RequirementAssignment>> Requirements { get; set; }
        public Dictionary<string, CapabilityAssignment> Capabilities { get; set; }
        public Dictionary<string, object> Interfaces { get; set; }
        public Dictionary<string, Artifact> Artifacts { get; set; }
        public object NodeFilter { get; set; }
        public string Copy { get; set; }
    }
}