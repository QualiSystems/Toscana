using System.Collections.Generic;
using Toscana.Exceptions;

namespace Toscana
{
    public class ToscaCapabilityType : ToscaObject
    {
        private ToscaCloudServiceArchive cloudServiceArchive;

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
        /// Sets archive that the node belongs to
        /// </summary>
        /// <param name="newCloudServiceArchive"></param>
        public void SetToscaCloudServiceArchive(ToscaCloudServiceArchive newCloudServiceArchive)
        {
            cloudServiceArchive = newCloudServiceArchive;
        }
    }
}