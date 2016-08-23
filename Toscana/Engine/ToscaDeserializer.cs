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
        T Deserialize(string tosca);
        T Deserialize(Stream stream);
    }

    internal class ToscaDeserializer<T> : IToscaDeserializer<T>
    {
        private readonly Deserializer deserializer;

        public ToscaDeserializer(ITypeConvertersFactory typeConvertersFactory)
        {
            deserializer = new Deserializer(namingConvention: new UnderscoredNamingConvention());

            foreach (var yamlTypeConverter in typeConvertersFactory.GetTypeConverter())
            {
                deserializer.RegisterTypeConverter(yamlTypeConverter);
            }
        }

        public T Deserialize(string tosca)
        {
            using (var stringReader = new StringReader(tosca))
            {
                return Deserialize(stringReader);
            }
        }

        /// <summary>
        /// Deserializes a stream of YAML to an instance of T
        /// </summary>
        /// <param name="stream">Stream </param>
        /// <returns></returns>
        public T Deserialize(Stream stream)
        {
            using (var stringReader = new StreamReader(stream))
            {
                return Deserialize(stringReader);
            }
        }

        private T Deserialize(TextReader stringReader)
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