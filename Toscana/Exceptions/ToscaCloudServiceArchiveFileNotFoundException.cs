using System;
using System.Runtime.Serialization;

namespace Toscana.Exceptions
{
    [Serializable]
    public class ToscaCloudServiceArchiveFileNotFoundException : ToscaBaseException
    {
        public ToscaCloudServiceArchiveFileNotFoundException()
        {
        }

        public ToscaCloudServiceArchiveFileNotFoundException(string message) : base(message)
        {
        }

        protected ToscaCloudServiceArchiveFileNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}