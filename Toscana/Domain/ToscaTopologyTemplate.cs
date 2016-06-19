using System.Collections.Generic;

namespace Toscana.Domain
{
    public class ToscaTopologyTemplate
    {
        public ToscaTopologyTemplate()
        {
            NodeTemplates = new Dictionary<string, ToscaNodeTemplate>();
            Inputs = new Dictionary<string, ToscaTopologyInput>();
            Outputs = new Dictionary<string, ToscaTopologyOutput>();
        }

        public Dictionary<string, ToscaNodeTemplate> NodeTemplates { get; set; }

        public Dictionary<string, ToscaTopologyInput> Inputs { get; set; }

        public Dictionary<string, ToscaTopologyOutput> Outputs { get; set; }
    }
}