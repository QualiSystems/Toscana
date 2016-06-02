using System.Collections.Generic;

namespace Toscana.Domain
{
    public class NodeTemplate
    {
        public string Type { get; set; }
        public Dictionary<string, Capability> Capabilities { get; set; }  
    }
}