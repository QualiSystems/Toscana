using System.Collections.Generic;
using YamlDotNet.Serialization;

namespace Toscana.Engine
{
    internal interface ITypeConvertersFactory
    {
        IEnumerable<IYamlTypeConverter> GetTypeConverter();
    }

    internal class TypeConvertersFactory : ITypeConvertersFactory
    {
        private readonly VersionTypeConverter[] typeConverters;

        public TypeConvertersFactory()
        {
            typeConverters = new[]
            {
                new VersionTypeConverter()
            };
        }

        public IEnumerable<IYamlTypeConverter> GetTypeConverter()
        {
            return typeConverters;
        }
    }
}