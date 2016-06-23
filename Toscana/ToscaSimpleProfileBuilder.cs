using System.Collections.Generic;
using System.Linq;
using Toscana.Engine;
using Toscana.Exceptions;

namespace Toscana
{
    public interface IToscaSimpleProfileBuilder
    {
        IToscaSimpleProfileBuilder Append(ToscaSimpleProfile toscaSimpleProfile);
        IToscaSimpleProfileBuilder Append(string toscaAsString);
        ToscaSimpleProfile Build();
    }

    public class ToscaSimpleProfileBuilder : IToscaSimpleProfileBuilder
    {
        private const string ToscaNodesRoot = "tosca.nodes.Root";
        private readonly List<ToscaSimpleProfile> toscaSimpleProfiles = new List<ToscaSimpleProfile>();

        public IToscaSimpleProfileBuilder Append(string toscaAsString)
        {
            toscaSimpleProfiles.Add(ToscaSimpleProfile.Parse(toscaAsString));
            return this;
        }

        public IToscaSimpleProfileBuilder Append(ToscaSimpleProfile toscaSimpleProfile)
        {
            toscaSimpleProfiles.Add(toscaSimpleProfile);
            return this;
        }

        public ToscaSimpleProfile Build()
        {
            var combinedTosca = new ToscaSimpleProfile();
            CombineNodeTypes(combinedTosca);
            BuildNodeTypeHierarchy(combinedTosca);
            var firstProfile = toscaSimpleProfiles.FirstOrDefault();
            if (firstProfile != null)
            {
                combinedTosca.Description = firstProfile.Description;
            }
            return combinedTosca;
        }

        private void CombineNodeTypes(ToscaSimpleProfile combinedTosca)
        {
            foreach (var simpleProfile in toscaSimpleProfiles)
            {
                foreach (var nodeType in simpleProfile.NodeTypes)
                {
                    if (combinedTosca.NodeTypes.ContainsKey(nodeType.Key))
                    {
                        throw new ToscaValidationException(string.Format("Node type {0} is duplicate", nodeType.Key));
                    }
                    combinedTosca.NodeTypes.Add(nodeType.Key, nodeType.Value);
                }
            }
            if (!combinedTosca.NodeTypes.ContainsKey(ToscaNodesRoot))
            {
                combinedTosca.NodeTypes.Add(ToscaNodesRoot, ToscaDefaultNodeTypes.GetRootNode());
            }
            foreach (var nodeType in combinedTosca.NodeTypes)
            {
                var derivedFrom = nodeType.Value.DerivedFrom;
                if (!nodeType.Value.IsRoot() && !combinedTosca.NodeTypes.ContainsKey(derivedFrom))
                {
                    throw new ToscaValidationException(string.Format("Definition of Node Type {0} is missing", derivedFrom));
                }
            }
        }

        private static void BuildNodeTypeHierarchy(ToscaSimpleProfile combinedTosca)
        {
            var nodeTypeWalker = new NodeTypeWalker(combinedTosca, nodeTypeName =>
            {
                var nodeType = combinedTosca.NodeTypes[nodeTypeName];
                if (nodeType.IsRoot()) return;

                var parentNode = combinedTosca.NodeTypes[nodeType.DerivedFrom];
                MergeCapabilities(parentNode, nodeType);
                MergeProperties(parentNode, nodeType);
                MergeInterfaces(parentNode, nodeType);
                MergeRequirements(parentNode, nodeType);
                MergeAttributes(parentNode, nodeType);
            });
            nodeTypeWalker.Walk(combinedTosca.NodeTypes.First(a=>a.Value.IsRoot()).Key);
        }

        private static void MergeProperties(ToscaNodeType parentNode, ToscaNodeType toscaNodeType)
        {
            foreach (var property in parentNode.Properties)
            {
                if (toscaNodeType.Properties.ContainsKey(property.Key))
                {
                    throw new ToscaValidationException(string.Format("Duplicate property definition of property {0}",
                        property.Key));
                }
                toscaNodeType.Properties.Add(property.Key, property.Value);
            }
        }

        private static void MergeInterfaces(ToscaNodeType parentNode, ToscaNodeType toscaNodeType)
        {
            foreach (var toscaInterface in parentNode.Interfaces)
            {
                if (toscaNodeType.Interfaces.ContainsKey(toscaInterface.Key))
                {
                    throw new ToscaValidationException(string.Format("Duplicate interface definition of interface {0}",
                        toscaInterface.Key));
                }
                toscaNodeType.Interfaces.Add(toscaInterface.Key, toscaInterface.Value);
            }
        }

        private static void MergeRequirements(ToscaNodeType parentNode, ToscaNodeType toscaNodeType)
        {
            foreach (var requirements in parentNode.Requirements)
            {
                foreach (var requirement in requirements)
                {
                    if (toscaNodeType.Requirements.Any(r=>r.ContainsKey(requirement.Key)))
                    {
                        throw new ToscaValidationException(string.Format("Duplicate requirement definition of requirement {0}",
                            requirement.Key));
                    }
                    toscaNodeType.Requirements.Add(new Dictionary<string, ToscaRequirement>{{requirement.Key, requirement.Value}});
                }
            }
        }

        private static void MergeAttributes(ToscaNodeType parentNode, ToscaNodeType toscaNodeType)
        {
            foreach (var attribute in parentNode.Attributes)
            {
                if (toscaNodeType.Attributes.ContainsKey(attribute.Key))
                {
                    throw new ToscaValidationException(string.Format("Duplicate attribute definition of attribute {0}",
                        attribute.Key));
                }
                toscaNodeType.Attributes.Add(attribute.Key, attribute.Value);
            }
        }

        private static void MergeCapabilities(ToscaNodeType parentNode, ToscaNodeType toscaNodeType)
        {
            foreach (var capability in parentNode.Capabilities)
            {
                if (toscaNodeType.Capabilities.ContainsKey(capability.Key))
                {
                    throw new ToscaValidationException(string.Format("Duplicate capability definition of capability {0}",
                        capability.Key));
                }
                toscaNodeType.Capabilities.Add(capability.Key, capability.Value);
            }
        }
    }
}