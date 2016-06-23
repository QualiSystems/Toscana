namespace Toscana.Engine
{
    public interface IToscaSimpleProfileParser
    {
        ToscaSimpleProfile Parse(string tosca);
    }

    public class ToscaSimpleProfileParser : IToscaSimpleProfileParser
    {
        private readonly IToscaValidator toscaValidator;
        private readonly IToscaSimpleProfileDeserializer toscaSimpleProfileDeserializer;

        public ToscaSimpleProfileParser(IToscaValidator validator, IToscaSimpleProfileDeserializer deserializer)
        {
            toscaValidator = validator;
            toscaSimpleProfileDeserializer = deserializer;
        }

        public ToscaSimpleProfile Parse(string tosca)
        {
            var toscaObject = toscaSimpleProfileDeserializer.Deserialize(tosca);
            toscaValidator.Validate(toscaObject);
            return toscaObject;
        }
    }
}