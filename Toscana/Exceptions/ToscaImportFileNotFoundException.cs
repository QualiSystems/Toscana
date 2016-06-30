using System;
using System.Runtime.Serialization;

namespace Toscana.Exceptions
{
    [Serializable]
    public class ToscaImportFileNotFoundException : ToscaBaseException
    {
        public ToscaImportFileNotFoundException()
        {
        }

        public ToscaImportFileNotFoundException(string message) : base(message)
        {
        }

        protected ToscaImportFileNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}