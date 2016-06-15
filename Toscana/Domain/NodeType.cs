using System;
using System.Collections.Generic;

namespace Toscana.Domain
{
    public class NodeType : ToscaObject
    {
        public NodeType()
        {
            Properties = new Dictionary<string, NodeProperty>();
            Attributes = new Dictionary<string, NodeAttribute>();
            Requirements = new List<Dictionary<string, Requirement>>();
            Capabilities = new Dictionary<string, Capability>();
            Interfaces = new Dictionary<string, Dictionary<string, object>>();
            Artifacts = new Dictionary<string, Artifact>();
        }

        public Version Version { get; set; }
        public string Description { get; set; }
        public Dictionary<string, NodeProperty> Properties { get; set; }
        public Dictionary<string, NodeAttribute> Attributes { get; set; }
        public List<Dictionary<string, Requirement>> Requirements { get; set; }
        public Dictionary<string, Capability> Capabilities { get; set; }
        public Dictionary<string, Dictionary<string, object>> Interfaces { get; set; }
        public Dictionary<string, Artifact> Artifacts { get; set; }
    }
}