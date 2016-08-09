using System;
using System.Runtime.Serialization;

namespace Toscana.Exceptions
{
    [Serializable]
    public class ToscaNodeTypeNotFoundException : ToscaBaseException
    {
        public ToscaNodeTypeNotFoundException()
        {
        }

        public ToscaNodeTypeNotFoundException(string message) : base(message)
        {
        }

        protected ToscaNodeTypeNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}