using System.Collections.Generic;

namespace Toscana
{
    public class ToscaCapabilityAssignment
    {
        public ToscaCapabilityAssignment()
        {
            Properties = new Dictionary<string, object>();
            Attributes = new Dictionary<string, ToscaAttributeAssignment>();
        }

        public Dictionary<string, object> Properties { get; set; }
        public Dictionary<string, ToscaAttributeAssignment> Attributes { get; set; }
    }
}