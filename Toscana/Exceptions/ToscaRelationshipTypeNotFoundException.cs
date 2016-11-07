using System;
using System.Runtime.Serialization;

namespace Toscana.Exceptions
{
    /// <summary>
    /// Thrown when relationship type not found
    /// </summary>
    [Serializable]
    public class ToscaRelationshipTypeNotFoundException : ToscaBaseException
    {
        /// <summary>
        /// Initializes an exception
        /// </summary>
        public ToscaRelationshipTypeNotFoundException()
        {
        }

        /// <summary>
        /// Initializes an exception with a message
        /// </summary>
        /// <param name="message"></param>
        public ToscaRelationshipTypeNotFoundException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes an exception from serialization info and streaming context
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        /// <exception cref="ArgumentNullException">The <paramref name="info" /> parameter is null. </exception>
        /// <exception cref="SerializationException">The class name is null or <see cref="P:System.Exception.HResult" /> is zero (0). </exception>
        public ToscaRelationshipTypeNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        /// <summary>
        /// Initializes an exception with an error message and inner exception
        /// </summary>
        /// <param name="message">Error message</param>
        /// <param name="exception">Inner exception</param>
        public ToscaRelationshipTypeNotFoundException(string message, Exception exception) : base(message, exception)
        {
        }
    }
}