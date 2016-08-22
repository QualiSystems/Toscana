using System.ComponentModel.DataAnnotations;

namespace Toscana
{
    /// <summary>
    /// The Requirement definition describes a named requirement (dependencies) 
    /// of a TOSCA Node Type or Node template which needs to be fulfilled by a matching 
    /// Capability definition declared by another TOSCA modelable entity.  
    /// The requirement definition may itself include the specific name of the fulfilling entity 
    /// (explicitly) or provide an abstract type, along with additional filtering characteristics, 
    /// that a TOSCA orchestrator can use to fulfill the capability at runtime (implicitly).
    /// </summary>
    public class ToscaRequirement
    {
        /// <summary>
        /// The required reserved keyname used that can be used to provide the name of a valid 
        /// Capability Type  that can fulfill the requirement.
        /// </summary>
        [Required(ErrorMessage = "capability is required on requirement")]
        public string Capability { get; set; }

        /// <summary>
        /// The optional reserved keyname used to provide the name of a valid Node Type that contains 
        /// the capability definition that can be used to fulfill the requirement.
        /// </summary>
        public string Node { get; set; }

        /// <summary>
        /// The optional reserved keyname used to provide the name of a valid Relationship Type to 
        /// construct when fulfilling the requirement.
        /// </summary>
        public string Relationship { get; set; }

        /// <summary>
        /// The optional minimum and maximum occurrences for the requirement. 
        /// Note: the keyword UNBOUNDED is also supported to represent any positive integer.
        /// </summary>
        public object[] Occurrences { get; set; }

        /// <summary>
        /// Initializes an instance of ToscaRequirement and set Capability property value
        /// </summary>
        /// <param name="capability">Capability to set</param>
        /// <returns>An instance of <see cref="ToscaRequirement"/></returns>
        public static implicit operator ToscaRequirement(string capability)
        {
            return new ToscaRequirement { Capability = capability };
        }
    }
}