using System;
using System.Runtime.Serialization;
using YamlDotNet.Core;

namespace Toscana.Exceptions
{
    /// <summary>
    /// Thrown when YAML parsing fails. Contains the original parsing exception as InnerException
    /// </summary>
    [Serializable]
    public class ToscaParsingException : ToscaBaseException
    {
        /// <summary>
        /// Initializes the exception
        /// </summary>
        public ToscaParsingException()
        {
        }

        /// <summary>
        /// Initializes the exception with a message
        /// </summary>
        /// <param name="message">Error message</param>
        public ToscaParsingException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes the exception with a message and inner exception
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public ToscaParsingException(string message, Exception exception) : base(message, exception)
        {
        }

        /// <summary>
        /// Initializes the exception with serialization info and streaming context
        /// </summary>
        /// <param name="info">Serialization info</param>
        /// <param name="context">Streaming context</param>
        /// <exception cref="ArgumentNullException">The <paramref name="info" /> parameter is null. </exception>
        /// <exception cref="SerializationException">The class name is null or <see cref="P:System.Exception.HResult" /> is zero (0). </exception>
        protected ToscaParsingException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}