using System;
using System.Collections.Generic;

namespace Toscana.Domain
{
    public class NodeType : ToscaObject
    {
        public Version Version { get; set; }
        public string Description { get; set; }
        public Dictionary<string, NodeProperty> Properties { get; set; }
        public Dictionary<string, NodeAttribute> Attributes { get; set; }
        public List<Dictionary<string, object>> Requirements { get; set; }
        public Dictionary<string, string> Capabilities { get; set; }
        public Dictionary<string, Dictionary<string, object>> Interfaces { get; set; }
        public Dictionary<string, Artifact> Artifacts { get; set; }
    }
}