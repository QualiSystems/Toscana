using System.ComponentModel.DataAnnotations;

namespace Toscana
{
    /// <summary>
    /// Represents TOSCA attribute assignment entity
    /// </summary>
    public class ToscaPropertyAssignment
    {
        /// <summary>
        /// Required value of value expresion of the property assignment
        /// </summary>
        [Required(ErrorMessage = "Value is required on property assignment.", AllowEmptyStrings = false)]
        public object Value { get; set; }

        /// <summary>
        /// Represents the optional description of the attribute.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Initializes an instance of ToscaPropertyAssignment with a value
        /// </summary>
        /// <param name="val">String value to be assigned to Value property</param>
        /// <returns>An instance of ToscaPropertyAssignment</returns>
        public static implicit operator ToscaPropertyAssignment(string val)
        {
            return new ToscaPropertyAssignment {Value = val};
        }
    }
}