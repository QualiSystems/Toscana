using System;
using System.Runtime.Serialization;

namespace Toscana.Exceptions
{
    /// <summary>
    /// Thrown when an artifact not found
    /// </summary>
    [Serializable]
    public class ArtifactNotFoundException : ToscaBaseException
    {
        /// <summary>
        /// Default constructor 
        /// </summary>
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