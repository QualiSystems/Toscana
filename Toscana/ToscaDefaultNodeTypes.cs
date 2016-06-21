using System.Collections.Generic;

namespace Toscana
{
    public static class ToscaDefaultNodeTypes
    {
        private static readonly ToscaNodeType RootNode;

        static ToscaDefaultNodeTypes()
        {
            RootNode = new ToscaNodeType
            {
                Description = "The TOSCA Node Type all other TOSCA base Node Types derive from"
            };
            RootNode.Attributes.Add("tosca_id", new ToscaAttributeDefinition {Type = "string"});
            RootNode.Attributes.Add("tosca_name", new ToscaAttributeDefinition {Type = "string"});
            RootNode.Attributes.Add("state", new ToscaAttributeDefinition {Type = "string"});
            RootNode.Capabilities.Add("feature", new ToscaCapability {Type = "tosca.capabilities.Node"});
            RootNode.Requirements.Add(new Dictionary<string, ToscaRequirement>
            {
                {
                    "dependency",
                    new ToscaRequirement
                    {
                        Capability = "tosca.capabilities.Node",
                        Node = "tosca.nodes.Root",
                        Relationship = "tosca.relationships.DependsOn",
                        Occurrences = new object[] {"0", "UNBOUNDED"}
                    }
                }
            });
        }

        public static ToscaNodeType GetRootNode()
        {
            return RootNode;
        }
    }
}