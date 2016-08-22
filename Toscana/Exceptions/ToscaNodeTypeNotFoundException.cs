using System;
using System.Runtime.Serialization;

namespace Toscana.Exceptions
{
    /// <summary>
    /// Thrown when Node Type not found
    /// </summary>
    [Serializable]
    public class ToscaNodeTypeNotFoundException : ToscaBaseException
    {
        /// <summary>
        /// Initializes the exception
        /// </summary>
        public ToscaNodeTypeNotFoundException()
        {
        }

        /// <summary>
        /// Initializes the exception with a message
        /// </summary>
        /// <param name="message"></param>
        public ToscaNodeTypeNotFoundException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes the exception with a message an an inner exception
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public ToscaNodeTypeNotFoundException(string message, Exception exception) : base(message, exception)
        {
        }

        /// <summary>
        /// Initializes the exception with serialization info and streaming context
        /// </summary>
        /// <param name="info">Serialization info</param>
        /// <param name="context">Streaming context</param>
        /// <exception cref="ArgumentNullException">The <paramref name="info" /> parameter is null. </exception>
        /// <exception cref="SerializationException">The class name is null or <see cref="P:System.Exception.HResult" /> is zero (0). </exception>
        protected ToscaNodeTypeNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}