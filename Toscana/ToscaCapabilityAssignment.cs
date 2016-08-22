using System.Collections.Generic;

namespace Toscana
{
    /// <summary>
    /// A capability assignment allows node template authors to assign values to properties 
    /// and attributes for a named capability definition that is part of a Node Template’s type definition.
    /// </summary>
    public class ToscaCapabilityAssignment
    {
        /// <summary>
        /// Ininializes an instance of <see cref="ToscaCapabilityAssignment"/>
        /// </summary>
        public ToscaCapabilityAssignment()
        {
            Properties = new Dictionary<string, object>();
            Attributes = new Dictionary<string, ToscaAttributeAssignment>();
        }

        /// <summary>
        /// An optional list of property definitions for the Capability definition.
        /// </summary>
        public Dictionary<string, object> Properties { get; set; }

        /// <summary>
        /// An optional list of attribute definitions for the Capability definition.
        /// </summary>
        public Dictionary<string, ToscaAttributeAssignment> Attributes { get; set; }
    }
}