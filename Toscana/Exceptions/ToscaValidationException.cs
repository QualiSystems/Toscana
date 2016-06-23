using System;
using System.Runtime.Serialization;

namespace Toscana.Exceptions
{
    [Serializable]
    public class ToscaValidationException : ToscaBaseException
    {
        public ToscaValidationException()
        {
        }

        public ToscaValidationException(string message) : base(message)
        {
        }

        protected ToscaValidationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}