using System;
using System.Runtime.Serialization;

namespace Toscana.Exceptions
{
    /// <summary>
    /// Thrown when an artifact not found
    /// </summary>
    [Serializable]
    public class ToscaArtifactNotFoundException : ToscaBaseException
    {
        /// <summary>
        /// Initialize an instance of the exception 
        /// </summary>
        public ToscaArtifactNotFoundException()
        {
        }

        /// <summary>
        /// Initializes an instance of an exception with a message
        /// </summary>
        /// <param name="message"></param>
        public ToscaArtifactNotFoundException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes an instance of the exception with a message and an inner exception
        /// </summary>
        /// <param name="message">Error message</param>
        /// <param name="exception">Inner exception</param>
        public ToscaArtifactNotFoundException(string message, Exception exception) : base(message, exception)
        {
        }

        /// <summary>
        /// Initializes and instance of exception
        /// </summary>
        /// <param name="info">Serialization info</param>
        /// <param name="context">Streaming context</param>
        /// <exception cref="ArgumentNullException">The <paramref name="info" /> parameter is null. </exception>
        /// <exception cref="SerializationException">The class name is null or <see cref="P:System.Exception.HResult" /> is zero (0). </exception>
        protected ToscaArtifactNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}