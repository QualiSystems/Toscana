using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Toscana
{
    /// <summary>
    /// A group definition defines a logical grouping of node templates, 
    /// typically for management purposes, but is separate from the application’s topology template.
    /// </summary>
    public class ToscaGroup
    {
        /// <summary>
        /// The required name of the group type the group definition is based upon.
        /// </summary>
        [Required(ErrorMessage = "Type is required on group.", AllowEmptyStrings = false)]
        public string Type { get; set; }

        /// <summary>
        /// The optional description for the group definition.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// An optional list of property value assignments for the group definition.
        /// </summary>
        public Dictionary<string, ToscaPropertyAssignment> Properties { get; set; }

        /// <summary>
        /// The optional list of one or more node template names that are members of this group definition.
        /// </summary>
        public string[] Members { get; set; }

        /// <summary>
        /// An optional list of named interface definitions for the group definition.
        /// </summary>
        public Dictionary<string, ToscaInterfaceDefinition> Interfaces { get; set; }
    }
}