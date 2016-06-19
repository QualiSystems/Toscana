using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Toscana
{
    public class ToscaCapability
    {
        [Required(ErrorMessage = "type is required on capability")]
        public string Type { get; set; }

        public string Description { get; set; }

        public Dictionary<string, ToscaNodeProperty> Properties { get; set; }

        public Dictionary<string, ToscaNodeAttribute> Attributes { get; set; }

        public static implicit operator ToscaCapability(string val)
        {
            return new ToscaCapability { Type = val };
        }
    }
}