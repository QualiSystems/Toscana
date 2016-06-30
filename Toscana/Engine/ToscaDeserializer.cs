using System.IO;
using Toscana.Common;
using Toscana.Exceptions;
using YamlDotNet.Core;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Toscana.Engine
{
    public interface IToscaDeserializer<out T>
    {
        T Deserialize(string tosca);
        T Deserialize(Stream stream);
    }

    public class ToscaDeserializer<T> : IToscaDeserializer<T>
    {
        private readonly Deserializer deserializer;

        public ToscaDeserializer()
        {
            deserializer = new Deserializer(namingConvention: new UnderscoredNamingConvention());
        }

        public T Deserialize(string tosca)
        {
            using (var stringReader = new StringReader(tosca))
            {
                return Deserialize(stringReader);
            }
        }

        public T Deserialize(Stream stream)
        {
            using (var streamReader = new StreamReader(stream))
            {
                return Deserialize(streamReader);
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
                throw new ToscaParsingException(yamlException.GetaAllMessages());
            }
        }
    }
}