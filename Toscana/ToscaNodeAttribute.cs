using System.ComponentModel.DataAnnotations;

namespace Toscana
{
    public class ToscaNodeAttribute
    {
        public ToscaNodeAttribute()
        {
            Status = ToscaPropertyStatus.supported;
        }

        [Required(ErrorMessage = "type is required on attribute")]
        public string Type { get; set; }

        public string Description { get; set; }

        public object Default { get; set; }

        public ToscaPropertyStatus Status { get; set; }

        public string EntrySchema { get; set; }
    }
}