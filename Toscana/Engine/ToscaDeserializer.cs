using System.IO;
using Toscana.Common;
using Toscana.Exceptions;
using YamlDotNet.Core;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Toscana.Engine
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
        private readonly Deserializer deserializer = new Deserializer(namingConvention: new UnderscoredNamingConvention());

        public ToscaDeserializer(ITypeConvertersFactory typeConvertersFactory)
        {
            foreach (var yamlTypeConverter in typeConvertersFactory.GetTypeConverter())
            {
                deserializer.RegisterTypeConverter(yamlTypeConverter);
            }
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