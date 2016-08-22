using System.Collections.Generic;

namespace Toscana
{
    /// <summary>
    /// A Relationship Template specifies the occurrence of a manageable relationship between node templates 
    /// as part of an application’s topology model that is defined in a TOSCA Service Template.  
    /// A Relationship template is an instance of a specified Relationship Type and can provide customized properties, 
    /// constraints or operations which override the defaults provided by its Relationship Type and its implementations.
    /// </summary>
    public class ToscaRelationshipTemplate
    {
        /// <summary>
        /// The required name of the Relationship Type the Relationship Template is based upon.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// An optional description for the Relationship Template.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// An optional list of property assignments for the Relationship Template.
        /// </summary>
        public Dictionary<string, ToscaPropertyAssignment> Properties { get; set; }

        /// <summary>
        /// An optional list of attribute assignments for the Relationship Template.
        /// </summary>
        public Dictionary<string, ToscaAttributeAssignment> Assignments { get; set; }

        /// <summary>
        /// An optional list of named interface definitions for the Node Template.
        /// </summary>
        public Dictionary<string, ToscaInterfaceDefinition> Interfaces { get; set; }

        /// <summary>
        /// The optional (symbolic) name of another relationship template 
        /// to copy into (all keynames and values) and use as a basis for this relationship template.
        /// </summary>
        public string Copy { get; set; }
    }
}