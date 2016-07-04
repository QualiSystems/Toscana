using System;
using System.Runtime.Serialization;

namespace Toscana.Exceptions
{
    [Serializable]
    public class ArtifactNotFoundException : ToscaBaseException
    {
        public ArtifactNotFoundException()
        {
        }

        public ArtifactNotFoundException(string message) : base(message)
        {
        }

        protected ArtifactNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}