using System.Collections.Generic;

namespace Toscana.Engine
{
    internal interface IToscaPropertyCombiner
    {
        Dictionary<string, List<ToscaProperty>> CombineProperties<T>(
            IToscaEntityWithProperties<T> toscaEntity)
            where T : IToscaEntityWithProperties<T>;
    }

    internal class ToscaPropertyCombiner : IToscaPropertyCombiner
    {
        public Dictionary<string, List<ToscaProperty>> CombineProperties<T>(
            IToscaEntityWithProperties<T> toscaEntity)
            where T : IToscaEntityWithProperties<T>
        {
            var combinedProperties = new Dictionary<string, List<ToscaProperty>>();
            for (var currNodeType = toscaEntity; currNodeType != null; currNodeType = currNodeType.GetDerivedFromEntity())
                foreach (var propertyKeyValue in currNodeType.Properties)
                {
                    if (!combinedProperties.ContainsKey(propertyKeyValue.Key))
                        combinedProperties.Add(propertyKeyValue.Key, new List<ToscaProperty>());
                    combinedProperties[propertyKeyValue.Key].Add(propertyKeyValue.Value);
                }
            return combinedProperties;
        }
    }
}