using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Toscana.Exceptions;

namespace Toscana
{
    /// <summary>
    /// A Node Type is a reusable entity that defines the type of one or more Node Templates. 
    /// As such, a Node Type defines the structure of observable properties via a Properties Definition, the Requirements and Capabilities of the node as well as its supported interfaces.
    /// </summary>
    public class ToscaNodeType : ToscaObject<ToscaNodeType>, IValidatableObject
    {
        /// <summary>
        /// Initializes a instance of ToscaNodeType
        /// </summary>
        public ToscaNodeType()
        {
            Properties = new Dictionary<string, ToscaPropertyDefinition>();
            Attributes = new Dictionary<string, ToscaAttributeDefinition>();
            Requirements = new List<Dictionary<string, ToscaRequirement>>();
            Capabilities = new Dictionary<string, ToscaCapability>();
            Interfaces = new Dictionary<string, Dictionary<string, object>>();
            Artifacts = new Dictionary<string, ToscaArtifact>();
        }

        /// <summary>
        /// An optional version for the Node Type definition.
        /// </summary>
        public Version Version { get; set; }

        /// <summary>
        /// An optional description for the Node Type.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// An optional list of property definitions for the Node Type.
        /// </summary>
        public Dictionary<string, ToscaPropertyDefinition> Properties { get; set; }

        /// <summary>
        /// An optional list of attribute definitions for the Node Type.
        /// </summary>
        public Dictionary<string, ToscaAttributeDefinition> Attributes { get; set; }

        /// <summary>
        /// An optional sequenced list of requirement definitions for the Node Type.
        /// </summary>
        public List<Dictionary<string, ToscaRequirement>> Requirements { get; set; }

        /// <summary>
        /// An optional list of capability definitions for the Node Type.
        /// </summary>
        public Dictionary<string, ToscaCapability> Capabilities { get; set; }

        /// <summary>
        /// An optional list of interface definitions supported by the Node Type.
        /// </summary>
        public Dictionary<string, Dictionary<string, object>> Interfaces { get; set; }

        /// <summary>
        /// An optional list of named artifact definitions for the Node Type.
        /// </summary>
        public Dictionary<string, ToscaArtifact> Artifacts { get; set; }

        /// <summary>
        /// Returns ToscaNodetype this Node type derives from.
        /// For root node type, null is returned
        /// </summary>
        /// <exception cref="ToscaNodeTypeNotFoundException">Thrown when Node Type pointed by Derived From not found</exception>
        public override ToscaNodeType Base
        {
            get
            {
                if (CloudServiceArchive == null || IsRoot()) return null;
                ToscaNodeType baseNodeType;
                if (CloudServiceArchive.NodeTypes.TryGetValue(DerivedFrom, out baseNodeType))
                {
                    return baseNodeType;
                }
                throw new ToscaNodeTypeNotFoundException(string.Format("Node type '{0}' not found", DerivedFrom));
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
                    allCapabilityTypes.Add(capability.Type, CloudServiceArchive.CapabilityTypes[capability.Type]);
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

        /// <summary>
        /// Returns all the properties of the node type and its ancestors
        /// </summary>
        /// <returns></returns>
        public IReadOnlyDictionary<string, ToscaPropertyDefinition> GetAllProperties()
        {
            var properties = new Dictionary<string, ToscaPropertyDefinition>();
            for (var currNodeType = this; currNodeType != null; currNodeType = currNodeType.Base)
            {
                foreach (var propertyKeyValue in currNodeType.Properties)
                {
                    properties.Add(propertyKeyValue.Key, propertyKeyValue.Value);
                }
            }
            return properties;
        }

        IEnumerable<ValidationResult> IValidatableObject.Validate(ValidationContext validationContext)
        {
            if (CloudServiceArchive == null) return Enumerable.Empty<ValidationResult>();

            return Artifacts.Where(toscaArtifact => !CloudServiceArchive.ContainsArtifact(toscaArtifact.Value.File))
                .Select(artifact => new ValidationResult(string.Format("Artifact '{0}' not found in Cloud Service Archive.", artifact.Value.File)))
                .ToList();
        }
    }
}