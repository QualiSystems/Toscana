using System;
using System.Runtime.Serialization;

namespace Toscana.Exceptions
{
    /// <summary>
    /// Thrown when parser for custom data type not found
    /// </summary>
    [Serializable]
    public class ToscaDataTypeParserNotFoundException : ToscaBaseException
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public ToscaDataTypeParserNotFoundException()
        {
        }

        /// <summary>
        /// Constructor with message
        /// </summary>
        /// <param name="message"></param>
        public ToscaDataTypeParserNotFoundException(string message) : base(message)
        {
        }

        /// <summary>
        /// Constructor for serialization
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public ToscaDataTypeParserNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        /// <summary>
        /// Constructor with message and an inner exception
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public ToscaDataTypeParserNotFoundException(string message, Exception exception) : base(message, exception)
        {
        }
    }
}