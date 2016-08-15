using System.Collections.Generic;
using Toscana.Exceptions;

namespace Toscana
{
    public class ToscaCapabilityType : ToscaObject
    {
        public ToscaCapabilityType()
        {
            Attributes = new Dictionary<string, ToscaAttributeDefinition>();
            Properties = new Dictionary<string, ToscaPropertyDefinition>();
            ValidSourceTypes = new string[0];
        }

        public string Version { get; set; }
        public string Description { get; set; }
        public Dictionary<string, ToscaPropertyDefinition> Properties { get; set; }
        public Dictionary<string, ToscaAttributeDefinition> Attributes { get; set; }
        public string[] ValidSourceTypes { get; set; }

        public ToscaCapabilityType Base
        {
            get
            {
                if (cloudServiceArchive == null || IsRoot()) return null;
                ToscaCapabilityType baseCapabilityType;
                if (cloudServiceArchive.CapabilityTypes.TryGetValue(DerivedFrom, out baseCapabilityType))
                {
                    return baseCapabilityType;
                }
                throw new ToscaCapabilityTypeNotFoundException(string.Format("Capability type '{0}' not found", DerivedFrom));
            }
        }

        /// <summary>
        /// Determinses whether the ToscaCapabilityType is derived from another capability type 
        /// </summary>
        /// <param name="capabilityTypeName">Name of the capability type to check</param>
        /// <returns>True if derives rom, false otherwise</returns>
        public bool IsDerivedFrom(string capabilityTypeName)
        {
            for (var currCaptype = this; !currCaptype.IsRoot(); currCaptype = currCaptype.Base)
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

    }
}