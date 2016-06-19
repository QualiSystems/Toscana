using System.ComponentModel.DataAnnotations;

namespace Toscana.Domain
{
    public class ToscaRequirement
    {
        [Required(ErrorMessage = "capability is required on requirement")]
        public string Capability { get; set; }
        public string Node { get; set; }
        public string Relationship { get; set; }
        public object[] Occurrences { get; set; }

        public static implicit operator ToscaRequirement(string val)
        {
            return new ToscaRequirement { Capability = val };
        }
    }
}