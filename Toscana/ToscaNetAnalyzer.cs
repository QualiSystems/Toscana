using System.IO;
using Toscana.Domain;
using Toscana.Engine;

namespace Toscana
{
    public class ToscaNetAnalyzer
    {
        private readonly IToscaValidator toscaValidator;
        private readonly IToscaParser toscaParser;

        public ToscaNetAnalyzer()
        {
            toscaValidator = new ToscaValidator();
            toscaParser = new ToscaParser();
        }

        public Tosca Analyze(string tosca)
        {
            var toscaObject = toscaParser.Parse(tosca);
            toscaValidator.Validate(toscaObject);
            return toscaObject;
        }

        public Tosca Analyze(TextReader textReader)
        {
            var toscaObject = toscaParser.Parse(textReader);
            toscaValidator.Validate(toscaObject);
            return toscaObject;
        }
    }
}