using System.IO;
using Toscana.Common;
using Toscana.Exceptions;
using YamlDotNet.Core;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Toscana
{
    internal interface IToscaDeserializer<out T>
    {
        /// <summary>
        /// Deserializes a stream of YAML to an instance of T
        /// </summary>
        /// <param name="stream">Stream </param>
        /// <returns></returns>
        /// <exception cref="ToscaParsingException">Thrown when YAML is not valid</exception>
        T Deserialize(Stream stream);
    }

    internal class ToscaDeserializer<T> : IToscaDeserializer<T>
    {
        private readonly IDeserializer deserializer;

        public ToscaDeserializer(ITypeConvertersFactory typeConvertersFactory)
        {
            var deserializerBuilder = new DeserializerBuilder()
                .WithNamingConvention(UnderscoredNamingConvention.Instance);

            foreach (var yamlTypeConverter in typeConvertersFactory.GetTypeConverter())
            {
                deserializerBuilder.WithTypeConverter(yamlTypeConverter);
            }

            deserializer = deserializerBuilder.Build();
        }

        /// <summary>
        /// Deserializes a stream of YAML to an instance of T
        /// </summary>
        /// <param name="stream">Stream </param>
        /// <returns></returns>
        /// <exception cref="ToscaParsingException">Thrown when YAML is not valid</exception>
        public T Deserialize(Stream stream)
        {
            using (var stringReader = new StreamReader(stream))
            {
                try
                {
                    return deserializer.Deserialize<T>(stringReader);
                }
                catch (YamlException yamlException)
                {
                    throw new ToscaParsingException(yamlException.GetaAllMessages(), yamlException);
                }
            }
        }
    }
}