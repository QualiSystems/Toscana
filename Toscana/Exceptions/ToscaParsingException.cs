using System;
using System.Runtime.Serialization;

namespace Toscana.Exceptions
{
    [Serializable]
    public class ToscaParsingException : ToscaBaseException
    {
        public ToscaParsingException()
        {
        }

        public ToscaParsingException(string message) : base(message)
        {
        }

        protected ToscaParsingException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}