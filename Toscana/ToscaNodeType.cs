using System;
using System.Collections.Generic;
using QuickGraph;

namespace Toscana
{
    public class ToscaNodeType : ToscaObject
    {
        public ToscaNodeType()
        {
            Properties = new Dictionary<string, ToscaPropertyDefinition>();
            Attributes = new Dictionary<string, ToscaAttributeDefinition>();
            Requirements = new List<Dictionary<string, ToscaRequirement>>();
            Capabilities = new Dictionary<string, ToscaCapability>();
            Interfaces = new Dictionary<string, Dictionary<string, object>>();
            Artifacts = new Dictionary<string, ToscaArtifact>();
        }

        public Version Version { get; set; }
        public string Description { get; set; }
        public Dictionary<string, ToscaPropertyDefinition> Properties { get; set; }
        public Dictionary<string, ToscaAttributeDefinition> Attributes { get; set; }
        public List<Dictionary<string, ToscaRequirement>> Requirements { get; set; }
        public Dictionary<string, ToscaCapability> Capabilities { get; set; }
        public Dictionary<string, Dictionary<string, object>> Interfaces { get; set; }
        public Dictionary<string, ToscaArtifact> Artifacts { get; set; }
    }
}