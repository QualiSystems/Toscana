using System.Collections.Generic;

namespace Toscana
{
    /// <summary>
    /// A Topology Template defines the structure of a service in the context of a Service Template. 
    /// A Topology Template consists of a set of Node Template and Relationship Template definitions 
    /// that together define the topology model of a service as a (not necessarily connected) directed graph.
    /// </summary>
    public class ToscaTopologyTemplate
    {
        /// <summary>
        /// Initializes an instance of ToscaTopologyTemplate
        /// </summary>
        public ToscaTopologyTemplate()
        {
            NodeTemplates = new Dictionary<string, ToscaNodeTemplate>();
            Inputs = new Dictionary<string, ToscaParameter>();
            Outputs = new Dictionary<string, ToscaParameter>();
        }

        /// <summary>
        /// The optional description for the Topology Template.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// An optional list of input parameters (i.e., as parameter definitions) for the Topology Template.
        /// </summary>
        public Dictionary<string, ToscaParameter> Inputs { get; set; }

        /// <summary>
        /// An optional list of node template definitions for the Topology Template.
        /// </summary>
        public Dictionary<string, ToscaNodeTemplate> NodeTemplates { get; set; }

        /// <summary>
        /// An optional list of relationship templates for the Topology Template.
        /// </summary>
        public Dictionary<string, ToscaRelationshipTemplate> RelationshipTemplates { get; set; }

        /// <summary>
        /// An optional list of Group definitions whose members are node templates defined within this same Topology Template.
        /// </summary>
        public Dictionary<string, ToscaGroup> Groups { get; set; }

        /// <summary>
        /// An optional list of Policy definitions for the Topology Template.
        /// </summary>
        public Dictionary<string, ToscaPolicy> Policies { get; set; }

        /// <summary>
        /// An optional list of output parameters (i.e., as parameter definitions) for the Topology Template.
        /// </summary>
        public Dictionary<string, ToscaParameter> Outputs { get; set; }

        /// <summary>
        /// An optional declaration that exports the topology template as an implementation of a Node type. 
        /// This also includes the mappings between the external Node Types named capabilities and requirements 
        /// to existing implementations of those capabilities and requirements on Node templates declared within the topology template.
        /// </summary>
        public object SubstitutionMappings { get; set; }
    }
}