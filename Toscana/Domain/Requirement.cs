using System.ComponentModel.DataAnnotations;

namespace Toscana.Domain
{
    public class Requirement
    {
        [Required(ErrorMessage = "capability is required on requirement")]
        public string Capability { get; set; }
        public string Node { get; set; }
        public string Relationship { get; set; }
        public object[] Occurrences { get; set; }

        public static implicit operator Requirement(string val)
        {
            return new Requirement { Capability = val };
        }
    }
}