using System.IO;
using Toscana.Domain;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Toscana
{
    public class ToscaNetAnalyzer
    {
        public Tosca Analyze(string tosca)
        {
            using (var stringReader = new StringReader(tosca))
            {
                var deserializer = new Deserializer(namingConvention: new CamelCaseNamingConvention());
                return deserializer.Deserialize<Tosca>(stringReader);
            }
        }
    }
}