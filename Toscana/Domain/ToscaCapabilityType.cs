using System.Collections.Generic;

namespace Toscana.Domain
{
    public class ToscaCapabilityType : ToscaObject
    {
        public ToscaCapabilityType()
        {
            Attributes = new Dictionary<string, ToscaNodeAttribute>();
            Properties = new Dictionary<string, ToscaNodeProperty>();
            ValidSourceTypes = new string[0];
        }

        public string Version { get; set; }
        public string Description { get; set; }
        public Dictionary<string, ToscaNodeProperty> Properties { get; set; }
        public Dictionary<string, ToscaNodeAttribute> Attributes { get; set; }
        public string[] ValidSourceTypes { get; set; }
    }
}