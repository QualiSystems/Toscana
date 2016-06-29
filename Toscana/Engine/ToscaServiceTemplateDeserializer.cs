using System.IO;
using Toscana.Common;
using Toscana.Exceptions;
using YamlDotNet.Core;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Toscana.Engine
{
    public interface IToscaServiceTemplateDeserializer
    {
        ToscaServiceTemplate Deserialize(string tosca);
    }

    public class ToscaServiceTemplateDeserializer : IToscaServiceTemplateDeserializer
    {
        private readonly Deserializer deserializer;

        public ToscaServiceTemplateDeserializer()
        {
            deserializer = new Deserializer(namingConvention: new UnderscoredNamingConvention());
        }

        public ToscaServiceTemplate Deserialize(string tosca)
        {
            using (var stringReader = new StringReader(tosca))
            {
                try
                {
                    return deserializer.Deserialize<ToscaServiceTemplate>(stringReader);
                }
                catch (YamlException yamlException)
                {
                    throw new ToscaParsingException(yamlException.GetaAllMessages());
                }
            }
        }
    }
}