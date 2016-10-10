using System.Collections.Generic;
using System.Linq;

namespace Toscana
{
    internal static class ToscaPropertyMerger
    {
        internal static Dictionary<string, ToscaPropertyDefinition> MergeProperties(Dictionary<string, List<ToscaPropertyDefinition>> combinedProperties)
        {
            var mergedProperties = new Dictionary<string, ToscaPropertyDefinition>();
            foreach (var property in combinedProperties)
            {
                var mergedProperty = property.Value.Last();
                if (property.Value.Count > 1)
                {
                    for (int i = property.Value.Count - 2; i >= 0; i--)
                    {
                        var overridingProperty = property.Value[i];
                        MergeProperty(overridingProperty, mergedProperty);
                    }
                }
                mergedProperties.Add(property.Key, mergedProperty);
            }
            return mergedProperties;
        }

        private static void MergeProperty(ToscaPropertyDefinition overridingProperty, ToscaPropertyDefinition mergedProperty)
        {
            if (overridingProperty.Default != null)
            {
                mergedProperty.Default = overridingProperty.Default;
            }
            if (overridingProperty.Description != null)
            {
                mergedProperty.Description = overridingProperty.Description;
            }
            if (overridingProperty.Constraints != null)
            {
                mergedProperty.Constraints = overridingProperty.Constraints;
            }
            mergedProperty.Required = overridingProperty.Required;
        }
    }
}