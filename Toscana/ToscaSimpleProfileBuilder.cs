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
                        throw new ToscanaValidationException(string.Format("Node type {0} is duplicate", nodeType.Key));
                    }
                    combinedTosca.NodeTypes.Add(nodeType.Key, nodeType.Value);
                }
            }
            foreach (var nodeType in combinedTosca.NodeTypes)
            {
                var derivedFrom = nodeType.Value.DerivedFrom;
                if (!nodeType.Value.IsRoot() && !combinedTosca.NodeTypes.ContainsKey(derivedFrom))
                {
                    throw new ToscanaValidationException(string.Format("Definition of Node Type {0} is missing", derivedFrom));
                }
            }
        }

        private static void BuildNodeTypeHierarchy(ToscaSimpleProfile combinedTosca)
        {
            var nodeTypeWalker = new NodeTypeWalker(combinedTosca, nodeType =>
            {
                if (!combinedTosca.NodeTypes.ContainsKey(nodeType))
                {
                    throw new ToscanaValidationException(string.Format("Definition of Node Type {0} is missing",
                        nodeType));
                }

                var toscaNodeType = combinedTosca.NodeTypes[nodeType];
                if (toscaNodeType.IsRoot()) return;

                var parentNode = combinedTosca.NodeTypes[toscaNodeType.DerivedFrom];
                foreach (var capability in parentNode.Capabilities)
                {
                    if (toscaNodeType.Capabilities.ContainsKey(capability.Key))
                    {
                        throw new ToscanaValidationException(string.Format("Duplicate capability definition of capability {0}", capability.Key));
                    }
                    toscaNodeType.Capabilities.Add(capability.Key, capability.Value);
                }
            });
            nodeTypeWalker.Walk(combinedTosca.NodeTypes.First(a=>a.Value.IsRoot()).Key);
        }
    }
}