using System.IO;
using Toscana.Common;
using Toscana.Exceptions;
using YamlDotNet.Core;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Toscana.Engine
{
    public interface IToscaSimpleProfileDeserializer
    {
        ToscaSimpleProfile Deserialize(string tosca);
    }

    public class ToscaSimpleProfileDeserializer : IToscaSimpleProfileDeserializer
    {
        private readonly Deserializer deserializer;

        public ToscaSimpleProfileDeserializer()
        {
            deserializer = new Deserializer(namingConvention: new UnderscoredNamingConvention());
        }

        public ToscaSimpleProfile Deserialize(string tosca)
        {
            using (var stringReader = new StringReader(tosca))
            {
                try
                {
                    return deserializer.Deserialize<ToscaSimpleProfile>(stringReader);
                }
                catch (YamlException yamlException)
                {
                    throw new ToscaParsingException(yamlException.GetaAllMessages());
                }
            }
        }
    }
}