using System;
using System.Runtime.Serialization;

namespace Toscana.Exceptions
{
    [Serializable]
    public class ToscaBaseException : Exception
    {
        public ToscaBaseException()
        {
        }

        public ToscaBaseException(string message) : base(message)
        {
        }

        protected ToscaBaseException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}