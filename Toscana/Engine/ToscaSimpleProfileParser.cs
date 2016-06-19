using Toscana.Domain;

namespace Toscana.Engine
{
    public interface IToscaSimpleProfileParser
    {
        ToscaSimpleProfile Parse(string tosca);
    }

    public class ToscaSimpleProfileParser : IToscaSimpleProfileParser
    {
        private readonly IToscaValidator toscaValidator;
        private readonly IToscaDeserializer toscaDeserializer;

        public ToscaSimpleProfileParser(IToscaValidator validator, IToscaDeserializer deserializer)
        {
            toscaValidator = validator;
            toscaDeserializer = deserializer;
        }

        public ToscaSimpleProfile Parse(string tosca)
        {
            var toscaObject = toscaDeserializer.Deserialize(tosca);
            toscaValidator.Validate(toscaObject);
            return toscaObject;
        }
    }
}