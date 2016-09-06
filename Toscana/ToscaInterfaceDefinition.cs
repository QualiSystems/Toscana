using System.Collections.Generic;

namespace Toscana
{
    /// <summary>
    /// An interface definition defines a named interface that can be associated 
    /// with a Node or Relationship Type
    /// </summary>
    public class ToscaInterfaceDefinition
    {
        /// <summary>
        /// Initializes an instance of <see cref="ToscaInterfaceDefinition"/>
        /// </summary>
        public ToscaInterfaceDefinition()
        {
            Inputs = new Dictionary<string, object>();
        }

        /// <summary>
        /// The optional list of input property definitions available to all defined operations for 
        /// interface definitions that are within TOSCA Node or Relationship Type definitions. 
        /// This includes when interface definitions are included as part of a Requirement definition in a Node Type.
        /// 
        /// 
        /// The optional list of input property assignments (i.e., parameters assignments) for 
        /// interface definitions that are within TOSCA Node or Relationship Template definitions. 
        /// This includes when interface definitions are referenced as part of a Requirement assignment in a Node Template.
        /// </summary>
        public Dictionary<string, object> Inputs { get; set; }
    }
}