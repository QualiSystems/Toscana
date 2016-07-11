using System.Collections.Generic;

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
                return cloudServiceArchive == null || IsRoot() ? null : cloudServiceArchive.CapabilityTypes[DerivedFrom];
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