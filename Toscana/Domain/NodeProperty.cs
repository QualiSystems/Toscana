using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Toscana.Domain
{
    public class NodeProperty
    {
        public NodeProperty()
        {
            Required = true;
        }

        [Required]
        public string Type { get; set; }
        public string Description { get; set; }
        public bool Required { get; set; }
        public object Default { get; set; }
        public PropertyStatus Status { get; set; }
        public List<Dictionary<string, object>> Constraints { get; set; }
        public string EntrySchema { get; set; }
    }
}