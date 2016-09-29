using System;
using System.Runtime.Serialization;

namespace Toscana.Exceptions
{
    /// <summary>
    /// Thrown when a file to load is not valid (not a zip file)
    /// </summary>
    [Serializable]
    public class ToscaInvalidFileException : ToscaBaseException
    {
        /// <summary>
        /// Initializes an exception
        /// </summary>
        public ToscaInvalidFileException()
        { }

        /// <summary>
        /// /// Initializes an exception with a message
        /// </summary>
        /// <param name="message"></param>
        public ToscaInvalidFileException(string message)
            : base(message)
        { }

        /// <summary>
        /// Initializes an exception from serialization info and streaming context
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        /// <exception cref="ArgumentNullException">The <paramref name="info" /> parameter is null. </exception>
        /// <exception cref="SerializationException">The class name is null or <see cref="P:System.Exception.HResult" /> is zero (0). </exception>
        public ToscaInvalidFileException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        { }

        /// <summary>
        /// Initializes an exception from with a custom message and an inner exception
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException">The inner exception that occured and is now handled</param>
        public ToscaInvalidFileException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}