using System.Collections.Generic;

namespace Toscana
{
    public class ToscaRelationshipType
    {
        public string DerivedFrom { get; set; }
        public string Version { get; set; }
        public string Description { get; set; }
        public List<ToscaPropertyDefinition> Properties { get; set; }
        public List<ToscaAttributeDefinition> Attributes { get; set; }
        public Dictionary<string, ToscaInterfaceDefinition> Interfaces { get; set; }
        public List<string> ValidTargetTypes { get; set; }
    }
}