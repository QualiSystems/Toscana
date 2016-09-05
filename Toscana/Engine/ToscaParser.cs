using System.IO;

namespace Toscana.Engine
{
    internal interface IToscaParser<out T>
    {
        T Parse(Stream stream);
    }

    internal class ToscaParser<T> : IToscaParser<T>
    {
        private readonly IToscaDeserializer<T> toscaDeserializer;
        private readonly IToscaValidator<T> toscaValidator;

        public ToscaParser(IToscaDeserializer<T> deserializer, IToscaValidator<T> validator)
        {
            toscaValidator = validator;
            toscaDeserializer = deserializer;
        }

        public T Parse(Stream stream)
        {
            var toscaObject = toscaDeserializer.Deserialize(stream);
            toscaValidator.Validate(toscaObject);
            return toscaObject;
        }
    }
}