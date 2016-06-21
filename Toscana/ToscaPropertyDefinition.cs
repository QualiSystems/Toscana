using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Toscana
{
    public class ToscaPropertyDefinition
    {
        public ToscaPropertyDefinition()
        {
            Required = true;
        }

        [Required(ErrorMessage = "type is required on property")]
        public string Type { get; set; }
        public string Description { get; set; }
        public bool Required { get; set; }
        public object Default { get; set; }
        public ToscaPropertyStatus Status { get; set; }
        public List<Dictionary<string, object>> Constraints { get; set; }
        public string EntrySchema { get; set; }
    }
}