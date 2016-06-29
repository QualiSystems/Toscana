namespace Toscana.Engine
{
    public interface IToscaServiceTemplateParser
    {
        ToscaServiceTemplate Parse(string tosca);
    }

    public class ToscaServiceTemplateParser : IToscaServiceTemplateParser
    {
        private readonly IToscaValidator toscaValidator;
        private readonly IToscaServiceTemplateDeserializer toscaServiceTemplateDeserializer;

        public ToscaServiceTemplateParser(IToscaValidator validator, IToscaServiceTemplateDeserializer deserializer)
        {
            toscaValidator = validator;
            toscaServiceTemplateDeserializer = deserializer;
        }

        public ToscaServiceTemplate Parse(string tosca)
        {
            var toscaObject = toscaServiceTemplateDeserializer.Deserialize(tosca);
            toscaValidator.Validate(toscaObject);
            return toscaObject;
        }
    }
}