using System;
using System.Runtime.Serialization;

namespace Toscana.Exceptions
{
    [Serializable]
    public class ToscanaBuildException : ToscanaBaseException
    {
        public ToscanaBuildException()
        {
        }

        public ToscanaBuildException(string message) : base(message)
        {
        }

        public ToscanaBuildException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ToscanaBuildException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}