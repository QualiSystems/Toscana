using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Toscana.Engine;
using Toscana.Exceptions;

namespace Toscana
{
    /// <summary>
    /// Represents TOSCA Capability type
    /// </summary>
    public class ToscaCapabilityType : ToscaObject<ToscaCapabilityType>, IToscaEntityWithProperties<ToscaCapabilityType>, IValidatableObject
    {
        /// <summary>
        /// Instantiates an instance of ToscaCapabilityType
        /// </summary>
        public ToscaCapabilityType()
        {
            Attributes = new Dictionary<string, ToscaAttribute>();
            Properties = new Dictionary<string, ToscaProperty>();
            ValidSourceTypes = new string[0];
        }

        /// <summary>
        /// An optional version for the Capability Type definition.
        /// </summary>
        public string Version { get; set; }
        
        /// <summary>
        /// An optional description for the Capability Type.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// An optional list of property definitions for the Capability Type.
        /// </summary>
        public Dictionary<string, ToscaProperty> Properties { get; set; }

        /// <summary>
        /// An optional list of attribute definitions for the Capability Type.
        /// </summary>
        public Dictionary<string, ToscaAttribute> Attributes { get; set; }

        /// <summary>
        /// Returns all the properties of the capability type and its ancestors
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ToscaCapabilityTypeNotFoundException">Thrown when this Capability Type derives from a non existing Capability Type</exception>
        public IReadOnlyDictionary<string, ToscaProperty> GetAllProperties()
        {
            return Bootstrapper.GetPropertyMerger().CombineAndMerge(this);
        }

        /// <summary>
        /// An optional list of one or more valid names of Node Types that are supported as valid sources of any relationship established to the declared Capability Type.
        /// </summary>
        public string[] ValidSourceTypes { get; set; }

        /// <summary>
        /// Returns CapabilityType that this Capability Type derives from.
        /// If this Capability Type is root, null will be returned
        /// If this Capability Type derives from a non existing Capability Type <see cref="ToscaCapabilityTypeNotFoundException"/> will be thrown
        /// </summary>
        /// <exception cref="ToscaCapabilityTypeNotFoundException">Thrown when this Capability Type derives from a non existing Capability Type</exception>
        public override ToscaCapabilityType GetDerivedFromEntity()
        {
            if (GetCloudServiceArchive() == null || IsRoot()) return null;
            ToscaCapabilityType baseCapabilityType;
            if (GetCloudServiceArchive().CapabilityTypes.TryGetValue(DerivedFrom, out baseCapabilityType))
            {
                return baseCapabilityType;
            }
            throw new ToscaCapabilityTypeNotFoundException(string.Format("Capability type '{0}' not found", DerivedFrom));
        }

        /// <summary>
        /// Determinses whether the ToscaCapabilityType is derived from another capability type 
        /// </summary>
        /// <param name="capabilityTypeName">Name of the capability type to check</param>
        /// <returns>True if derives rom, false otherwise</returns>
        public bool IsDerivedFrom(string capabilityTypeName)
        {
            for (var currCaptype = this; !currCaptype.IsRoot(); currCaptype = currCaptype.GetDerivedFromEntity())
            {
                if (currCaptype.DerivedFrom == capabilityTypeName) return true;
            }
            return false;
        }

        /// <summary>
        /// Sets DerivedFrom to point to tosca.nodes.Root if it's not set
        /// </summary>
        /// <param name="name">Node type name</param>
        public override void SetDerivedFromToRoot(string name)
        {
            if (name != ToscaDefaults.ToscaCapabilitiesRoot && IsRoot())
            {
                DerivedFrom = ToscaDefaults.ToscaCapabilitiesRoot;
            }
        }

        IEnumerable<ValidationResult> IValidatableObject.Validate(ValidationContext validationContext)
        {
            return ValidateCircularDependency();
        }
    }
}