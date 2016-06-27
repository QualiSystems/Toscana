using System.IO;
using Toscana.Common;
using Toscana.Exceptions;
using YamlDotNet.Core;
using YamlDotNet.Serialization;

namespace Toscana.Engine
{
    public interface IToscaMetadataDeserializer
    {
        ToscaMetadata Deserialize(string tosca);
        ToscaMetadata Deserialize(Stream stream);
    }

    public class ToscaMetadataDeserializer : IToscaMetadataDeserializer
    {
        private readonly Deserializer deserializer;

        public ToscaMetadataDeserializer()
        {
            deserializer = new Deserializer();
        }

        public ToscaMetadata Deserialize(string tosca)
        {
            using (var stringReader = new StringReader(tosca))
            {
                return Deserialize(stringReader);
            }
        }

        public ToscaMetadata Deserialize(Stream stream)
        {
            using (var stringReader = new StreamReader(stream))
            {
                return Deserialize(stringReader);
            }
        }

        private ToscaMetadata Deserialize(TextReader stringReader)
        {
            try
            {
                return deserializer.Deserialize<ToscaMetadata>(stringReader);
            }
            catch (YamlException yamlException)
            {
                throw new ToscaParsingException(yamlException.GetaAllMessages());
            }
        }
    }
}