namespace Toscana
{
    /// <summary>
    /// A Requirement assignment allows template authors to provide either concrete names of TOSCA templates 
    /// or provide abstract selection criteria for providers to use to find matching TOSCA templates 
    /// that are used to fulfill a named requirement’s declared TOSCA Node Type.
    /// </summary>
    public class ToscaRequirementAssignment
    {
        /// <summary>
        /// The optional reserved keyname used to provide the name of either a: 
        /// * Capability definition within a target node template that can fulfill the requirement.
        /// * Capability Type that the provider will use to select a type-compatible target node template 
        /// to fulfill the requirement at runtime.
        /// </summary>
        public string Capability { get; set; }

        /// <summary>
        /// The optional reserved keyname used to identify the target node of a relationship.specifically, it is used to provide either a: 
        /// * Node Template name that can fulfill the target node requirement. 
        /// * Node Type name that the provider will use to select a type-compatible node template to fulfill the requirement at runtime.
        /// </summary>
        public string Node { get; set; }


        /// <summary>
        /// The optional reserved keyname used to provide the name of either a: 
        /// * Relationship Template to use to relate the source node to the(capability in the) target node when fulfilling the requirement. 
        /// * Relationship Type that the provider will use to select a type-compatible relationship template to relate the source node to the target node at runtime.
        /// </summary>
        public string Relationship { get; set; }

        /// <summary>
        /// The optional filter definition that TOSCA orchestrators or providers would use to select 
        /// a type-compatible target node that can fulfill the associated abstract requirement at runtime.
        /// </summary>
        public string NodeFilter { get; set; }

        /// <summary>
        /// Initializes an instance of <see cref="ToscaRequirementAssignment" /> and set Node property
        /// </summary>
        /// <param name="node">Node value to set</param>
        /// <returns>An instance of <see cref="ToscaRequirementAssignment"/></returns>
        public static implicit operator ToscaRequirementAssignment(string node)
        {
            return new ToscaRequirementAssignment {Node = node};
        }
    }
}