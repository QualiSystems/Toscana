using System.Collections.Generic;

namespace Toscana.Domain
{
    public class NodeType
    {
        public Dictionary<string, NodeProperty> Properties { get; set; }
        public Dictionary<string, NodeAttribute> Attributes { get; set; }
        public Dictionary<string, string> Capabilities { get; set; }
        public List<Dictionary<string, object>> Requirements { get; set; }
    }
}