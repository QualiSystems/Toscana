using System;
using System.Runtime.Serialization;

namespace Toscana.Exceptions
{
    [Serializable]
    public class ToscanaBaseException : Exception
    {
        public ToscanaBaseException()
        {
        }

        public ToscanaBaseException(string message) : base(message)
        {
        }

        public ToscanaBaseException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ToscanaBaseException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}