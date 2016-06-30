using System;
using System.Runtime.Serialization;

namespace Toscana.Exceptions
{
    [Serializable]
    public class ToscaMetadataFileNotFound : ToscaBaseException
    {
        public ToscaMetadataFileNotFound()
        {
        }

        public ToscaMetadataFileNotFound(string message) : base(message)
        {
        }

        protected ToscaMetadataFileNotFound(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}