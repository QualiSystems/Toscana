using System.IO;
using Toscana.Domain;
using Toscana.Domain.DigitalUnits;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Toscana.Engine
{
    public interface IToscaParser
    {
        Tosca Parse(string tosca);
    }

    public class ToscaParser : IToscaParser
    {
        private readonly Deserializer deserializer;

        public ToscaParser()
        {
            deserializer = CreateDeserializer();
        }

        public Tosca Parse(string tosca)
        {
            using (var stringReader = new StringReader(tosca))
            {
                return deserializer.Deserialize<Tosca>(stringReader);
            }
        }

        private static Deserializer CreateDeserializer()
        {
            var toscaDeserializer = new Deserializer(namingConvention: new UnderscoredNamingConvention());
            toscaDeserializer.RegisterTypeConverter(new DigitalStorageConverter());
            return toscaDeserializer;
        }
    }
}