using System.Collections.Generic;

namespace Toscana.Engine
{
    internal interface IToscaPropertyCombiner
    {
        Dictionary<string, List<ToscaPropertyDefinition>> CombineProperties<T>(
            IToscaEntityWithProperties<T> toscaEntity)
            where T : IToscaEntityWithProperties<T>;
    }

    internal class ToscaPropertyCombiner : IToscaPropertyCombiner
    {
        public Dictionary<string, List<ToscaPropertyDefinition>> CombineProperties<T>(
            IToscaEntityWithProperties<T> toscaEntity)
            where T : IToscaEntityWithProperties<T>
        {
            var combinedProperties = new Dictionary<string, List<ToscaPropertyDefinition>>();
            for (var currNodeType = toscaEntity; currNodeType != null; currNodeType = currNodeType.Base)
                foreach (var propertyKeyValue in currNodeType.Properties)
                {
                    if (!combinedProperties.ContainsKey(propertyKeyValue.Key))
                        combinedProperties.Add(propertyKeyValue.Key, new List<ToscaPropertyDefinition>());
                    combinedProperties[propertyKeyValue.Key].Add(propertyKeyValue.Value);
                }
            return combinedProperties;
        }
    }
}