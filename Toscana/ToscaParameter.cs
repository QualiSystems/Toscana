using System.Collections.Generic;

namespace Toscana
{
    /// <summary>
    /// A parameter definition is essentially a TOSCA property definition; 
    /// however, it also allows a value to be assigned to it (as for a TOSCA property assignment). 
    /// In addition, in the case of output parameters, it can optionally inherit the data type of 
    /// the value assigned to it rather than have an explicit data type defined for it.
    /// </summary>
    public class ToscaParameter
    {
        /// <summary>
        /// Initializes an instance of <see cref="ToscaParameter"/>
        /// </summary>
        public ToscaParameter()
        {
            Required = true;
        }

        /// <summary>
        /// The type-compatible value to assign to the named parameter.  
        /// Parameter values may be provided as the result from the evaluation of an expression or a function.
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        /// The required data type for the parameter. 
        /// 
        /// Note: This keyname is required for a TOSCA Property definition, but is not for a TOSCA Parameter definition.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// The optional description for the parameter.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// An optional key that declares a parameter as required (true) or not (false).
        /// </summary>
        public bool Required { get; set; }

        /// <summary>
        /// An optional key that may provide a value to be used as a default if not provided by another means.
        /// </summary>
        public object Default { get; set; }

        /// <summary>
        /// The optional status of the parameter relative to the specification or implementation. 
        /// </summary>
        public ToscaPropertyStatus Status { get; set; }

        /// <summary>
        /// The optional list of sequenced constraint clauses for the parameter.
        /// </summary>
        public List<Dictionary<string, object>> Constraints { get; set; }

        /// <summary>
        /// The optional key that is used to declare the name of the Datatype definition for entries of set types such as the TOSCA list or map.
        /// </summary>
        public string EntrySchema { get; set; }

        /// <summary>
        /// String representation of Value property.
        /// Returns empty string when Value is null
        /// </summary>
        public string StringValue
        {
            get { return Value == null ? string.Empty : Value.ToString(); }
        }

        /// <summary>
        /// String representation of Default property.
        /// Returns empty string when default is null
        /// </summary>
        public string StringDefaultValue
        {
            get { return Default == null ? string.Empty : Default.ToString(); }
        }
    }
}