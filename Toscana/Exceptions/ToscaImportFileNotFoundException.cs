using System;
using System.Runtime.Serialization;

namespace Toscana.Exceptions
{
    /// <summary>
    /// Thrown when a file pointed by imports directive inside a TOSCA Service Template YAML file not found
    /// </summary>
    [Serializable]
    public class ToscaImportFileNotFoundException : ToscaBaseException
    {
        /// <summary>
        /// Initializes the exception
        /// </summary>
        public ToscaImportFileNotFoundException()
        {
        }

        /// <summary>
        /// Initializes the exception with a message
        /// </summary>
        /// <param name="message"></param>
        public ToscaImportFileNotFoundException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes the exception with a message and inner exception
        /// </summary>
        /// <param name="message">Error message</param>
        /// <param name="exception">Inner exception</param>
        public ToscaImportFileNotFoundException(string message, Exception exception) : base(message, exception)
        {
        }

        /// <summary>
        /// Initializes the exception from serialization info and streaming context. Used implicitly during deserialization
        /// </summary>
        /// <param name="info">Serialization info</param>
        /// <param name="context">Streaming context</param>
        /// <exception cref="ArgumentNullException">The <paramref name="info" /> parameter is null. </exception>
        /// <exception cref="SerializationException">The class name is null or <see cref="P:System.Exception.HResult" /> is zero (0). </exception>
        protected ToscaImportFileNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}