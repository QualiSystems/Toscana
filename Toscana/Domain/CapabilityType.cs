using System.Collections.Generic;

namespace Toscana.Domain
{
    public class CapabilityType : ToscaObject
    {
        public string Version { get; set; }
        public string Description { get; set; }
        public Dictionary<string, NodeProperty> Properties { get; set; }
        public Dictionary<string, NodeAttribute> Attributes { get; set; }
        public string[] ValidSourceTypes { get; set; }
    }
}