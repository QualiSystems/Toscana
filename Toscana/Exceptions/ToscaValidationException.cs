using System;
using System.Runtime.Serialization;

namespace Toscana.Exceptions
{
    /// <summary>
    /// Thrown when TOSCA validation fails
    /// </summary>
    [Serializable]
    public class ToscaValidationException : ToscaBaseException
    {
        /// <summary>
        /// Initializes the exception
        /// </summary>
        public ToscaValidationException()
        {
        }

        /// <summary>
        /// Initializes the exception with a message
        /// </summary>
        /// <param name="message"></param>
        public ToscaValidationException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes the exception with a message an an inner exception
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public ToscaValidationException(string message, Exception exception) : base(message, exception)
        {
        }

        /// <summary>
        /// Initializes the exception with serialization info and streaming context
        /// </summary>
        /// <param name="info">Serialization info</param>
        /// <param name="context">Streaming context</param>
        /// <exception cref="ArgumentNullException">The <paramref name="info" /> parameter is null. </exception>
        /// <exception cref="SerializationException">The class name is null or <see cref="P:System.Exception.HResult" /> is zero (0). </exception>
        protected ToscaValidationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}