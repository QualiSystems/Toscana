using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Toscana
{
    /// <summary>
    /// A capability definition defines a named, typed set of data that can be associated with 
    /// Node Type or Node Template to describe a transparent capability or feature of the software 
    /// component the node describes.
    /// </summary>
    public class ToscaCapability
    {
        /// <summary>
        /// Initializes an instance of ToscaCapability class
        /// </summary>
        public ToscaCapability()
        {
            Properties = new Dictionary<string, ToscaPropertyDefinition>();
            Attributes = new Dictionary<string, ToscaAttributeDefinition>();
        }

        /// <summary>
        /// The required name of the Capability Type the capability definition is based upon.
        /// </summary>
        [Required(ErrorMessage = "type is required on capability", AllowEmptyStrings = false)]
        public string Type { get; set; }

        /// <summary>
        /// The optional description of the Capability definition.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// An optional list of property definitions for the Capability definition.
        /// </summary>
        public Dictionary<string, ToscaPropertyDefinition> Properties { get; set; }

        /// <summary>
        /// An optional list of attribute definitions for the Capability definition.
        /// </summary>
        public Dictionary<string, ToscaAttributeDefinition> Attributes { get; set; }

        /// <summary>
        /// Initializes an instance of of <see cref="ToscaCapability"/> and set its Type property
        /// </summary>
        /// <param name="type">Value to set to Type property</param>
        /// <returns>An instance of ToscaCapability</returns>
        public static implicit operator ToscaCapability(string type)
        {
            return new ToscaCapability { Type = type };
        }
    }
}