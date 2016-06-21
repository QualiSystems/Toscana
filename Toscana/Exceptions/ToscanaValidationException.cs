using System;
using System.Runtime.Serialization;

namespace Toscana.Exceptions
{
    [Serializable]
    public class ToscanaValidationException : ToscanaBaseException
    {
        public ToscanaValidationException()
        {
        }

        public ToscanaValidationException(string message) : base(message)
        {
        }

        protected ToscanaValidationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}