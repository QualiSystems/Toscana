using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Toscana.Engine;
using Toscana.Exceptions;

namespace Toscana
{
    /// <summary>
    /// A Node Type is a reusable entity that defines the type of one or more Node Templates. 
    /// As such, a Node Type defines the structure of observable properties via a Properties Definition, the Requirements and Capabilities of the node as well as its supported interfaces.
    /// </summary>
    public class ToscaNodeType : ToscaObject<ToscaNodeType>, IToscaEntityWithProperties<ToscaNodeType>, IValidatableObject
    {
        private readonly ToscaPropertyCombiner toscaPropertyCombiner;

        /// <summary>
        /// Initializes a instance of ToscaNodeType
        /// </summary>
        public ToscaNodeType()
        {
            Properties = new Dictionary<string, ToscaProperty>();
            Attributes = new Dictionary<string, ToscaAttribute>();
            Requirements = new List<Dictionary<string, ToscaRequirement>>();
            Capabilities = new Dictionary<string, ToscaCapability>();
            Interfaces = new Dictionary<string, Dictionary<string, object>>();
            Artifacts = new Dictionary<string, ToscaArtifact>();
            toscaPropertyCombiner = new ToscaPropertyCombiner();
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
        public Dictionary<string, ToscaProperty> Properties { get; set; }

        /// <summary>
        /// An optional list of attribute definitions for the Node Type.
        /// </summary>
        public Dictionary<string, ToscaAttribute> Attributes { get; set; }

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
        public override ToscaNodeType GetDerivedFromEntity()
        {
            if (GetCloudServiceArchive() == null || IsRoot()) return null;
            ToscaNodeType baseNodeType;
            if (GetCloudServiceArchive().NodeTypes.TryGetValue(DerivedFrom, out baseNodeType))
            {
                return baseNodeType;
            }
            throw new ToscaNodeTypeNotFoundException(string.Format("Node type '{0}' not found", DerivedFrom));
        }

        /// <summary>
        /// Returns requirements of the ToscaNodeType and its ancestors
        /// </summary>
        /// <returns></returns>
        public List<ToscaRequirement> GetAllRequirements()
        {
            var requirements = new List<ToscaRequirement>();
            for (var currNodeType = this; currNodeType != null; currNodeType = currNodeType.GetDerivedFromEntity())
            {
                foreach (var requirementKeyValue in currNodeType.Requirements.SelectMany(r => r))
                {
                    requirements.Add(requirementKeyValue.Value);
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

            for (var currNodeType = this; currNodeType != null; currNodeType = currNodeType.GetDerivedFromEntity())
            {
                foreach (var capability in currNodeType.Capabilities.Values)
                {
                    allCapabilityTypes.Add(capability.Type, GetCloudServiceArchive().CapabilityTypes[capability.Type]);
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
        /// <exception cref="ToscaNodeTypeNotFoundException">Thrown when Node Type pointed by Derived From not found</exception>
        public IReadOnlyDictionary<string, ToscaProperty> GetAllProperties()
        {
            return Bootstrapper.Current.GetPropertyMerger().CombineAndMerge(this);
        }

        IEnumerable<ValidationResult> IValidatableObject.Validate(ValidationContext validationContext)
        {
            if (GetCloudServiceArchive() == null) return Enumerable.Empty<ValidationResult>();

            var validationResults = Artifacts.Where(toscaArtifact => !GetCloudServiceArchive().ContainsArtifact(toscaArtifact.Value.File))
                .Select(artifact => new ValidationResult(string.Format("Artifact '{0}' not found in Cloud Service Archive.", artifact.Value.File)))
                .ToList();

            // no need to add circular validation results, as they have been already added in Validate of CloudServiceArchive
            if (ValidateCircularDependency().Any())
            {
                return validationResults;
            }

            var combineProperties = toscaPropertyCombiner.CombineProperties(this);
            validationResults.AddRange(
                combineProperties.Where(
                    combineProperty => combineProperty.Value.Select(p => p.Type).Distinct().Count() > 1)
                    .Select(
                        combineProperty =>
                            new ValidationResult(
                                string.Format(
                                    "Property '{0}' has different type when overriden, which is not allowed",
                                    combineProperty.Key))));
            return validationResults;
        }
    }
}