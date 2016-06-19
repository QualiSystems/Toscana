using Toscana.Domain;
using Toscana.Engine;

namespace Toscana
{
    public class ToscaNetAnalyzer
    {
        private readonly IToscaValidator toscaValidator;
        private readonly IToscaDeserializer toscaDeserializer;

        public ToscaNetAnalyzer()
        {
            toscaValidator = new ToscaValidator();
            toscaDeserializer = new ToscaDeserializer();
        }

        public ToscaSimpleProfile Analyze(string tosca)
        {
            var toscaObject = toscaDeserializer.Deserialize(tosca);
            toscaValidator.Validate(toscaObject);
            return toscaObject;
        }
    }
}