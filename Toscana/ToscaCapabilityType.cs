using System.Collections.Generic;

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
    }
}