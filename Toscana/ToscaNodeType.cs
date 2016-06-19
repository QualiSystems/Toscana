using System;
using System.Collections.Generic;

namespace Toscana
{
    public class ToscaNodeType : ToscaObject
    {
        public ToscaNodeType()
        {
            Properties = new Dictionary<string, ToscaNodeProperty>();
            Attributes = new Dictionary<string, ToscaNodeAttribute>();
            Requirements = new List<Dictionary<string, ToscaRequirement>>();
            Capabilities = new Dictionary<string, ToscaCapability>();
            Interfaces = new Dictionary<string, Dictionary<string, object>>();
            Artifacts = new Dictionary<string, ToscaArtifact>();
        }

        public Version Version { get; set; }
        public string Description { get; set; }
        public Dictionary<string, ToscaNodeProperty> Properties { get; set; }
        public Dictionary<string, ToscaNodeAttribute> Attributes { get; set; }
        public List<Dictionary<string, ToscaRequirement>> Requirements { get; set; }
        public Dictionary<string, ToscaCapability> Capabilities { get; set; }
        public Dictionary<string, Dictionary<string, object>> Interfaces { get; set; }
        public Dictionary<string, ToscaArtifact> Artifacts { get; set; }
    }
}