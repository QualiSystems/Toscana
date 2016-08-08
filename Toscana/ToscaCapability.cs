using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Toscana
{
    public class ToscaCapability
    {
        public ToscaCapability()
        {
            Properties = new Dictionary<string, ToscaPropertyDefinition>();
            Attributes = new Dictionary<string, ToscaAttributeDefinition>();
        }

        [Required(ErrorMessage = "type is required on capability")]
        public string Type { get; set; }

        public string Description { get; set; }

        public Dictionary<string, ToscaPropertyDefinition> Properties { get; set; }

        public Dictionary<string, ToscaAttributeDefinition> Attributes { get; set; }

        public static implicit operator ToscaCapability(string val)
        {
            return new ToscaCapability { Type = val };
        }
    }
}