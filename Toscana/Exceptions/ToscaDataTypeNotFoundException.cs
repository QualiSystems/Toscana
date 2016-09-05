using System;
using System.Runtime.Serialization;

namespace Toscana.Exceptions
{
    /// <summary>
    /// Thrown when data type not found
    /// </summary>
    [Serializable]
    public class ToscaDataTypeNotFoundException : ToscaBaseException
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public ToscaDataTypeNotFoundException()
        {
        }

        /// <summary>
        /// Initializes an exception with a message
        /// </summary>
        /// <param name="message"></param>
        public ToscaDataTypeNotFoundException(string message) : base(message)
        {
        }

        /// <summary>
        /// Deserializes an exception from serialization context 
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        /// <exception cref="ArgumentNullException">The <paramref name="info" /> parameter is null. </exception>
        /// <exception cref="SerializationException">The class name is null or <see cref="P:System.Exception.HResult" /> is zero (0). </exception>
        public ToscaDataTypeNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        /// <summary>
        /// Initializes an exception with an inner exception
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public ToscaDataTypeNotFoundException(string message, Exception exception) : base(message, exception)
        {
        }
    }
}