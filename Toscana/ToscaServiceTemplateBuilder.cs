using System.Collections.Generic;
using System.IO;
using System.Linq;
using Toscana.Engine;
using Toscana.Exceptions;

namespace Toscana
{
    internal interface IToscaServiceTemplateBuilder
    {
        ToscaServiceTemplateBuilder Append(Stream stream);
        IToscaServiceTemplateBuilder Append(ToscaServiceTemplate toscaServiceTemplate);
        ToscaServiceTemplate Build();
    }

    internal class ToscaServiceTemplateBuilder : IToscaServiceTemplateBuilder
    {
        private readonly List<ToscaServiceTemplate> toscaServiceTemplates = new List<ToscaServiceTemplate>();

        public ToscaServiceTemplateBuilder Append(Stream stream)
        {
            toscaServiceTemplates.Add(ToscaServiceTemplate.Load(stream));
            return this;
        }

        public IToscaServiceTemplateBuilder Append(ToscaServiceTemplate toscaServiceTemplate)
        {
            toscaServiceTemplates.Add(toscaServiceTemplate);
            return this;
        }

        public ToscaServiceTemplate Build()
        {
            var combinedTosca = new ToscaServiceTemplate();
            CombineNodeTypes(combinedTosca);
            BuildNodeTypeHierarchy(combinedTosca);
            var firstProfile = toscaServiceTemplates.FirstOrDefault();
            if (firstProfile != null)
            {
                combinedTosca.Description = firstProfile.Description;
            }
            return combinedTosca;
        }

        private void CombineNodeTypes(ToscaServiceTemplate combinedTosca)
        {
            foreach (var simpleProfile in toscaServiceTemplates)
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
            if (!combinedTosca.NodeTypes.ContainsKey(ToscaDefaults.ToscaNodesRoot))
            {
                combinedTosca.NodeTypes.Add(ToscaDefaults.ToscaNodesRoot, ToscaDefaults.GetRootNodeType());
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

        private static void BuildNodeTypeHierarchy(ToscaServiceTemplate combinedTosca)
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