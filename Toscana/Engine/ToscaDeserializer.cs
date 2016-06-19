using System.IO;
using Toscana.Domain;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Toscana.Engine
{
    public interface IToscaDeserializer
    {
        ToscaSimpleProfile Deserialize(string tosca);
    }

    public class ToscaDeserializer : IToscaDeserializer
    {
        private readonly Deserializer deserializer;

        public ToscaDeserializer()
        {
            deserializer = new Deserializer(namingConvention: new UnderscoredNamingConvention());
        }

        public ToscaSimpleProfile Deserialize(string tosca)
        {
            using (var stringReader = new StringReader(tosca))
            {
                return deserializer.Deserialize<ToscaSimpleProfile>(stringReader);
            }
        }
    }
}