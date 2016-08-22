using System;
using System.Runtime.Serialization;

namespace Toscana.Exceptions
{
    /// <summary>
    /// Base TOSCA Exception class to be used as a base class in Toscana for all the exceptions
    /// </summary>
    [Serializable]
    public class ToscaBaseException : Exception
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public ToscaBaseException()
        {
        }

        /// <summary>
        /// Instantiates an exception with the message
        /// </summary>
        /// <param name="message"></param>
        public ToscaBaseException(string message) : base(message)
        {
        }

        /// <summary>
        /// Constructor used to deserialize exception object
        /// </summary>
        /// <param name="info">Serialization info</param>
        /// <param name="context">Streaming context</param>
        /// <exception cref="ArgumentNullException">The <paramref name="info" /> parameter is null. </exception>
        /// <exception cref="SerializationException">The class name is null or <see cref="P:System.Exception.HResult" /> is zero (0). </exception>
        public ToscaBaseException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        /// <summary>
        /// Initializes the exception with a message and inner exception
        /// </summary>
        /// <param name="message">Error message</param>
        /// <param name="exception">Inner exception</param>
        public ToscaBaseException(string message, Exception exception): base(message, exception)
        {
            
        }
    }
}