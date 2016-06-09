using System;
using System.Runtime.Serialization;

namespace Toscana.Exceptions
{
    [Serializable]
    public class ToscaValidationException : ToscanaBaseException
    {
        public ToscaValidationException()
        {
        }

        public ToscaValidationException(string message) : base(message)
        {
        }

        public ToscaValidationException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ToscaValidationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}