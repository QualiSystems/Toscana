using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Toscana.Domain
{
    public class Capability
    {
        [Required(ErrorMessage = "type is required on capability")]
        public string Type { get; set; }

        public string Description { get; set; }

        public Dictionary<string, NodeProperty> Properties { get; set; }

        public Dictionary<string, NodeAttribute> Attributes { get; set; }

        public static implicit operator Capability(string val)
        {
            return new Capability { Type = val };
        }
    }
}