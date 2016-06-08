using System.Collections.Generic;

namespace Toscana.Domain
{
    public class Tosca
    {
        public string ToscaDefinitionsVersion { get; set; }
        public string Description { get; set; }
        public TopologyTemplate TopologyTemplate { get; set; }
        public Dictionary<string, NodeType> NodeTypes { get; set; }
    }
}