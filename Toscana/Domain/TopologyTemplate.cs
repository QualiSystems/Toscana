using System.Collections.Generic;

namespace Toscana.Domain
{
    public class TopologyTemplate
    {
        public Dictionary<string, NodeTemplate> NodeTemplates { get; set; }

        public Dictionary<string, TopologyInput> Inputs { get; set; }

        public Dictionary<string, TopologyOutput> Outputs { get; set; }
    }
}