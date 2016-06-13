using System.ComponentModel.DataAnnotations;

namespace Toscana.Domain
{
    public class NodeAttribute
    {
        public NodeAttribute()
        {
            Status = PropertyStatus.supported;
        }

        [Required(ErrorMessage = "type is required on attribute")]
        public string Type { get; set; }

        public string Description { get; set; }

        public object Default { get; set; }

        public PropertyStatus Status { get; set; }

        public string EntrySchema { get; set; }
    }
}