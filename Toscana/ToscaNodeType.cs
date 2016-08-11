using System;
using System.Collections.Generic;
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
                throw new ToscaNodeTypeNotFoundException(string.Format("Node type '{0}' not found", DerivedFrom));
            }
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
    }
}