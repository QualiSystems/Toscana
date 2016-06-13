namespace Toscana.Domain
{
    public class RequirementAssignment
    {
        public string Capability { get; set; }
        public string Node { get; set; }
        public string Relationship { get; set; }
        public string NodeFilter { get; set; }

        public static implicit operator RequirementAssignment(string val)
        {
            return new RequirementAssignment {Node = val};
        }
    }
}