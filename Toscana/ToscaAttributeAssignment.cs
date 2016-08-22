namespace Toscana
{
    /// <summary>
    /// Represents TOSCA Assignment that allows assigning values to attributes
    /// </summary>
    public class ToscaAttributeAssignment
    {
        /// <summary>
        /// Represents the optional description of the attribute.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Represent the type-compatible value to assign to the named attribute.  
        /// Attribute values may be provided as the result from the evaluation of an expression or a function.
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        /// Initializes an instance of ToscaAttributeAssignment and set its value to val
        /// </summary>
        /// <param name="val">string value to set to the Value property</param>
        /// <returns></returns>
        public static implicit operator ToscaAttributeAssignment(string val)
        {
            return new ToscaAttributeAssignment {Value = val};
        }
    }
}