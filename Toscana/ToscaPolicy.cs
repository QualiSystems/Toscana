using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Toscana
{
    /// <summary>
    /// A policy definition defines a policy that can be associated 
    /// with a TOSCA topology or top-level entity definition (e.g., group definition, node template, etc.).
    /// </summary>
    public class ToscaPolicy
    {
        /// <summary>
        /// Initializes an instance of <see cref="ToscaPolicy"/>
        /// </summary>
        public ToscaPolicy()
        {
            Properties = new Dictionary<string, ToscaPropertyAssignment>();
            Targets = new string[0];
        }

        /// <summary>
        /// The required name of the policy type the policy definition is based upon.
        /// </summary>
        [Required(ErrorMessage = "Type is required on policy.", AllowEmptyStrings = false)]
        public string Type { get; set; }

        /// <summary>
        /// The optional description for the policy definition.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// An optional list of property value assignments for the policy definition.
        /// </summary>
        public Dictionary<string, ToscaPropertyAssignment> Properties { get; set; }

        /// <summary>
        /// An optional list of valid Node Templates or Groups the Policy can be applied to.
        /// </summary>
        public string[] Targets { get; set; }
    }
}