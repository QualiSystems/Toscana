using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Toscana
{
    /// <summary>
    /// A property definition defines a named, typed value and related data that 
    /// can be associated with an entity defined in this specification (e.g., Node Types, 
    /// Relationship Types, Capability Types, etc.).  
    /// Properties are used by template authors to provide input values to TOSCA entities which 
    /// indicate their “desired state” when they are instantiated.  The value of a property can be 
    /// retrieved using the get_property function within TOSCA Service Templates.
    /// </summary>
    public class ToscaPropertyDefinition
    {
        /// <summary>
        /// Initializes an instance of ToscaPropertyDefinition
        /// </summary>
        public ToscaPropertyDefinition()
        {
            Required = true;
        }

        /// <summary>
        /// The required data type for the property.
        /// </summary>
        [Required(ErrorMessage = "type is required on property", AllowEmptyStrings = false)]
        public string Type { get; set; }

        /// <summary>
        /// The optional description for the property.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// An optional key that declares a property as required (true) or not (false).
        /// </summary>
        public bool Required { get; set; }

        /// <summary>
        /// An optional key that may provide a value to be used as a default if not provided by another means.
        /// </summary>
        public object Default { get; set; }

        /// <summary>
        /// The optional status of the property relative to the specification or implementation. 
        /// </summary>
        public ToscaPropertyStatus Status { get; set; }

        /// <summary>
        /// The optional list of sequenced constraint clauses for the property.
        /// </summary>
        public List<Dictionary<string, object>> Constraints { get; set; }

        /// <summary>
        /// The optional key that is used to declare the name of the Datatype definition for entries of set types such as the TOSCA list or map.
        /// </summary>
        public string EntrySchema { get; set; }

        /// <summary>
        /// The optional list of tags
        /// </summary>
        public List<string> Tags { get; set; }

        /// <summary>
        /// String representation of Default property.
        /// Returns empty string when default is null
        /// </summary>
        public string StringValue
        {
            get { return Default == null ? string.Empty : Default.ToString(); }
        }
    }
}