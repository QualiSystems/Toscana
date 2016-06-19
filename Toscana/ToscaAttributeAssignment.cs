namespace Toscana
{
    public class ToscaAttributeAssignment
    {
        public string Description { get; set; }
        public object Value { get; set; }

        public static implicit operator ToscaAttributeAssignment(string val)
        {
            return new ToscaAttributeAssignment {Value = val};
        }
    }
}