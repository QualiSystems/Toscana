namespace Toscana.Domain
{
    public class AttributeAssignment
    {
        public string Description { get; set; }
        public object Value { get; set; }

        public static implicit operator AttributeAssignment(string val)
        {
            return new AttributeAssignment {Value = val};
        }
    }
}