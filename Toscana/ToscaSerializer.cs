using System.IO;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Toscana
{
    internal interface IToscaSerializer<in T>
    {
        void Serialize(Stream stream, T toscaObject);
    }

    internal class ToscaSerializer<T> : IToscaSerializer<T>
    {
        private readonly ISerializer serializer;

        public ToscaSerializer(ITypeConvertersFactory typeConvertersFactory)
        {
            var serializerBuilder = new SerializerBuilder()
                .WithNamingConvention(UnderscoredNamingConvention.Instance)
                .ConfigureDefaultValuesHandling(DefaultValuesHandling.OmitNull);

            foreach (var yamlTypeConverter in typeConvertersFactory.GetTypeConverter())
            {
                serializerBuilder.WithTypeConverter(yamlTypeConverter);
            }

            serializer = serializerBuilder.Build();
        }

        public void Serialize(Stream stream, T toscaObject)
        {
            using (var writer = new StreamWriter(stream))
            {
                serializer.Serialize(writer, toscaObject);
                writer.Flush();
            }
        }
    }
}