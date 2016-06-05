using System.Collections.Generic;

namespace Toscana.Domain
{
    public class TopologyTemplate
    {
        public TopologyTemplate()
        {
            NodeTemplates = new Dictionary<string, NodeTemplate>();
            Inputs = new Dictionary<string, TopologyInput>();
            Outputs = new Dictionary<string, TopologyOutput>();
        }

        public Dictionary<string, NodeTemplate> NodeTemplates { get; set; }

        public Dictionary<string, TopologyInput> Inputs { get; set; }

        public Dictionary<string, TopologyOutput> Outputs { get; set; }
    }
}