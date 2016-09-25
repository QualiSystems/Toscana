using System.IO;
using Toscana.Exceptions;

namespace Toscana.Engine
{
    internal interface IToscaParser<out T>
    {
        /// <summary>
        /// Parses TOSCA entity from the stream
        /// </summary>
        /// <param name="stream"></param>
        /// <returns>TOSCA entity</returns>
        /// <exception cref="ToscaParsingException">Thrown when YAML is not valid</exception>
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

        /// <summary>
        /// Parses TOSCA entity from the stream
        /// </summary>
        /// <param name="stream"></param>
        /// <returns>TOSCA entity</returns>
        /// <exception cref="ToscaParsingException">Tosca entity is null or not valid</exception>
        public T Parse(Stream stream)
        {
            var toscaObject = toscaDeserializer.Deserialize(stream);
            toscaValidator.Validate(toscaObject);
            return toscaObject;
        }
    }
}