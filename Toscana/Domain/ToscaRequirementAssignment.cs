namespace Toscana.Domain
{
    public class ToscaRequirementAssignment
    {
        public string Capability { get; set; }
        public string Node { get; set; }
        public string Relationship { get; set; }
        public string NodeFilter { get; set; }

        public static implicit operator ToscaRequirementAssignment(string val)
        {
            return new ToscaRequirementAssignment {Node = val};
        }
    }
}