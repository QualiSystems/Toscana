using System.IO;
using Toscana.Domain;
using Toscana.Domain.DigitalUnits;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Toscana
{
    public class ToscaNetAnalyzer
    {
        private readonly Deserializer deserializer;

        public ToscaNetAnalyzer()
        {
            deserializer = CreateDeserializer();
        }

        public Tosca Analyze(string tosca)
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