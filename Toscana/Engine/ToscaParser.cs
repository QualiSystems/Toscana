using System.IO;
using Toscana.Domain;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Toscana.Engine
{
    public interface IToscaParser
    {
        ToscaSimpleProfile Parse(string tosca);
        ToscaSimpleProfile Parse(TextReader textReader);
    }

    public class ToscaParser : IToscaParser
    {
        private readonly Deserializer deserializer;

        public ToscaParser()
        {
            deserializer = CreateDeserializer();
        }

        public ToscaSimpleProfile Parse(string tosca)
        {
            using (var stringReader = new StringReader(tosca))
            {
                return Parse(stringReader);
            }
        }

        public ToscaSimpleProfile Parse(TextReader textReader)
        {
            return deserializer.Deserialize<ToscaSimpleProfile>(textReader);
        }

        private static Deserializer CreateDeserializer()
        {
            var toscaDeserializer = new Deserializer(namingConvention: new UnderscoredNamingConvention());
            return toscaDeserializer;
        }
    }
}