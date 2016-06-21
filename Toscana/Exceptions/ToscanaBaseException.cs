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

        protected ToscanaBaseException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}