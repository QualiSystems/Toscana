using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Toscana
{
    /// <summary>
    /// A Node Template specifies the occurrence of a manageable software component as part of an application’s topology model which is 
    /// defined in a TOSCA Service Template.  A Node template is an instance of a specified Node Type and can provide customized properties, 
    /// constraints or operations which override the defaults provided by its Node Type and its implementations.
    /// </summary>
    public class ToscaNodeTemplate
    {
        /// <summary>
        /// Initalizes an instance of ToscaNodeTemplate
        /// </summary>
        public ToscaNodeTemplate()
        {
            Artifacts = new Dictionary<string, ToscaArtifact>();
            Properties = new Dictionary<string, object>();
            Attributes = new Dictionary<string, ToscaAttributeAssignment>();
            Capabilities = new Dictionary<string, ToscaCapabilityAssignment>();
            Directives = new string[0];
            Interfaces = new Dictionary<string, object>();
            Requirements = new List<Dictionary<string, ToscaRequirementAssignment>>();
        }

        /// <summary>
        /// The required name of the Node Type the Node Template is based upon.
        /// </summary>
        [Required(ErrorMessage = "type is required on node_template")]
        public string Type { get; set; }

        /// <summary>
        /// An optional description for the Node Template.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// An optional list of directive values to provide processing instructions to orchestrators and tooling.
        /// </summary>
        public string[] Directives { get; set; }

        /// <summary>
        /// An optional list of property value assignments for the Node Template.
        /// </summary>
        public Dictionary<string, object> Properties { get; set; }

        /// <summary>
        /// An optional list of attribute value assignments for the Node Template.
        /// </summary>
        public Dictionary<string, ToscaAttributeAssignment> Attributes { get; set; }

        /// <summary>
        /// An optional sequenced list of requirement assignments for the Node Template.
        /// </summary>
        public List<Dictionary<string, ToscaRequirementAssignment>> Requirements { get; set; }

        /// <summary>
        /// An optional list of capability assignments for the Node Template.
        /// </summary>
        public Dictionary<string, ToscaCapabilityAssignment> Capabilities { get; set; }

        /// <summary>
        /// An optional list of named interface definitions for the Node Template.
        /// </summary>
        public Dictionary<string, object> Interfaces { get; set; }

        /// <summary>
        /// An optional list of named artifact definitions for the Node Template.
        /// </summary>
        public Dictionary<string, ToscaArtifact> Artifacts { get; set; }

        /// <summary>
        /// The optional filter definition that TOSCA orchestrators would use to select the correct target node.  This keyname is only valid if the directive has the value of “selectable” set.
        /// </summary>
        public object NodeFilter { get; set; }

        /// <summary>
        /// The optional (symbolic) name of another node template to copy into (all keynames and values) and use as a basis for this node template.
        /// </summary>
        public string Copy { get; set; }
    }
}