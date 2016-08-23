using System.IO;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Toscana.Engine
{
    internal interface IToscaSerializer<in T>
    {
        void Serialize(TextWriter writer, T toscaObject);
    }

    internal class ToscaSerializer<T> : IToscaSerializer<T>
    {
        private readonly Serializer serializer = new Serializer(namingConvention: new UnderscoredNamingConvention());

        public ToscaSerializer(ITypeConvertersFactory typeConvertersFactory)
        {
            foreach (var yamlTypeConverter in typeConvertersFactory.GetTypeConverter())
            {
                serializer.RegisterTypeConverter(yamlTypeConverter);
            }
        }

        public void Serialize(TextWriter writer, T toscaObject)
        {
            serializer.Serialize(writer, toscaObject);
        }
    }
}