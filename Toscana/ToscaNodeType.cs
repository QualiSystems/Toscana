using System;
using System.Collections.Generic;
using System.Linq;
using Toscana.Exceptions;

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

        public ToscaNodeType Base
        {
            get
            {
                if (cloudServiceArchive == null || IsRoot()) return null;
                ToscaNodeType baseNodeType;
                if (cloudServiceArchive.NodeTypes.TryGetValue(DerivedFrom, out baseNodeType))
                {
                    return baseNodeType;
                }
                throw new ToscaNodeTypeNotFoundException(String.Format("Node type '{0}' not found", DerivedFrom));
            }
        }

        /// <summary>
        /// Returns requirements of the ToscaNodeType and its ancestors
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, ToscaRequirement> GetAllRequirements()
        {
            var requirements = new Dictionary<string, ToscaRequirement>();
            for (var currNodeType = this; currNodeType != null; currNodeType = currNodeType.Base)
            {
                foreach (var requirementKeyValue in currNodeType.Requirements.SelectMany(r => r))
                {
                    requirements.Add(requirementKeyValue.Key, requirementKeyValue.Value);
                }
            }
            return requirements;
        }
        /// <summary>
        /// Returns capability types of the ToscaNodeType and its ancestors
        /// </summary>
        /// <returns>Caapbility types of node type and its ancestors</returns>
        public Dictionary<string, ToscaCapabilityType> GetAllCapabilityTypes()
        {
            var allCapabilityTypes = new Dictionary<string, ToscaCapabilityType>();

            for (var currNodeType = this; currNodeType != null; currNodeType = currNodeType.Base)
            {
                foreach (var capability in currNodeType.Capabilities.Values)
                {
                    allCapabilityTypes.Add(capability.Type, cloudServiceArchive.CapabilityTypes[capability.Type]);
                }
            }
            return allCapabilityTypes;
        }

        /// <summary>
        /// Adds a requirements
        /// </summary>
        /// <param name="name">Requirement name</param>
        /// <param name="toscaRequirement">Requirement to add</param>
        public void AddRequirement(string name, ToscaRequirement toscaRequirement)
        {
            Requirements.Add(new Dictionary<string, ToscaRequirement>
            {
                {name, toscaRequirement}
            });
        }

        /// <summary>
        /// Sets DerivedFrom to point to tosca.nodes.Root if it's not set
        /// </summary>
        /// <param name="name">Node type name</param>
        public override void SetDerivedFromToRoot(string name)
        {
            if (name != ToscaDefaults.ToscaNodesRoot && IsRoot())
            {
                DerivedFrom = ToscaDefaults.ToscaNodesRoot;
            }
        }
    }
}