using System.ComponentModel.DataAnnotations;

namespace Toscana
{
    /// <summary>
    /// An attribute definition defines a named, typed value that can be associated with an entity 
    /// defined in this specification (e.g., a Node, Relationship or Capability Type).  
    /// Specifically, it is used to expose the “actual state” of some property of a TOSCA entity 
    /// after it has been deployed and instantiated (as set by the TOSCA orchestrator).  
    /// Attribute values can be retrieved via the get_attribute function from the instance model 
    /// and used as values to other entities within TOSCA Service Templates.
    /// </summary>
    public class ToscaAttributeDefinition
    {
        /// <summary>
        /// Initializes an instance of <see cref="ToscaAttributeDefinition"/>
        /// </summary>
        public ToscaAttributeDefinition()
        {
            Status = ToscaPropertyStatus.supported;
        }

        /// <summary>
        /// The required data type for the attribute.
        /// </summary>
        [Required(ErrorMessage = "type is required on attribute")]
        public string Type { get; set; }

        /// <summary>
        /// The optional description for the attribute.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// An optional key that may provide a value to be used as a default if not provided by another means. 
        /// This value SHALL be type compatible with the type declared by the property definition’s type keyname.
        /// </summary>
        public object Default { get; set; }

        /// <summary>
        /// The optional status of the attribute relative to the specification or implementation.  
        /// </summary>
        public ToscaPropertyStatus Status { get; set; }

        /// <summary>
        /// The optional key that is used to declare the name of the Datatype definition for entries of set types such as the TOSCA list or map.
        /// </summary>
        public string EntrySchema { get; set; }
    }
}