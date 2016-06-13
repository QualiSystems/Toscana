using System.Collections.Generic;

namespace Toscana.Domain
{
    public class CapabilityAssignment
    {
        public Dictionary<string, object> Properties { get; set; }
        public Dictionary<string, AttributeAssignment> Attributes { get; set; }
    }
}