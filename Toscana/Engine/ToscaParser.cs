using System.IO;

namespace Toscana.Engine
{
    public interface IToscaParser<out T>
    {
        T Parse(string tosca);
        T Parse(Stream stream);
    }

    public class ToscaParser<T> : IToscaParser<T>
    {
        private readonly IToscaDeserializer<T> toscaDeserializer;
        private readonly IToscaValidator<T> toscaValidator;

        public ToscaParser(IToscaDeserializer<T> deserializer, IToscaValidator<T> validator)
        {
            toscaValidator = validator;
            toscaDeserializer = deserializer;
        }

        public T Parse(string tosca)
        {
            var toscaObject = toscaDeserializer.Deserialize(tosca);
            toscaValidator.Validate(toscaObject);
            return toscaObject;
        }

        public T Parse(Stream stream)
        {
            var toscaObject = toscaDeserializer.Deserialize(stream);
            toscaValidator.Validate(toscaObject);
            return toscaObject;
        }
    }
}