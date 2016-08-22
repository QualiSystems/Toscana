using System;
using System.Runtime.Serialization;

namespace Toscana.Exceptions
{
    /// <summary>
    /// Thrown when TOSCA.meta file is not found inside TOSCA Cloud Service Archive (CSAR) ZIP file
    /// </summary>
    [Serializable]
    public class ToscaMetadataFileNotFound : ToscaBaseException
    {
        /// <summary>
        /// Initializes the exception
        /// </summary>
        public ToscaMetadataFileNotFound()
        {
        }

        /// <summary>
        /// Initializes the exception with a message
        /// </summary>
        /// <param name="message"></param>
        public ToscaMetadataFileNotFound(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes the exception with a message and an inner exception
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public ToscaMetadataFileNotFound(string message, Exception exception) : base(message, exception)
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        /// <exception cref="SerializationException">The class name is null or <see cref="P:System.Exception.HResult" /> is zero (0). </exception>
        protected ToscaMetadataFileNotFound(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}