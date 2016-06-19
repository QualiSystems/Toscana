using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

// ReSharper disable All

namespace Toscana.Domain
{
    public class ToscaNodeTemplate
    {
        public ToscaNodeTemplate()
        {
            Artifacts = new Dictionary<string, ToscaArtifact>();
            Properties = new Dictionary<string, object>();
            Attributes = new Dictionary<string, ToscaAttributeAssignment>();
            Capabilities = new Dictionary<string, ToscaCapabilityAssignment>();
            Directives = new string[0];
            Interfaces = new Dictionary<string, object>();
            Requirements = new List<Dictionary<string, ToscaRequirementAssignment>>();
        }

        [Required(ErrorMessage = "type is required on node_template")]
        public string Type { get; set; }

        public string Description { get; set; }

        public string[] Directives { get; set; }

        public Dictionary<string, object> Properties { get; set; }
        public Dictionary<string, ToscaAttributeAssignment> Attributes { get; set; }
        public List<Dictionary<string, ToscaRequirementAssignment>> Requirements { get; set; }
        public Dictionary<string, ToscaCapabilityAssignment> Capabilities { get; set; }
        public Dictionary<string, object> Interfaces { get; set; }
        public Dictionary<string, ToscaArtifact> Artifacts { get; set; }
        public object NodeFilter { get; set; }
        public string Copy { get; set; }
    }
}