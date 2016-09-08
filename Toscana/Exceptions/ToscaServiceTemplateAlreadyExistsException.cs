using System;
using System.Runtime.Serialization;

namespace Toscana.Exceptions
{
    /// <summary>
    /// Thrown when Service Template already exists
    /// </summary>
    [Serializable]
    public class ToscaServiceTemplateAlreadyExistsException : ToscaBaseException
    {
        /// <summary>
        /// Initializes an instance of <see cref="ToscaServiceTemplateAlreadyExistsException"/>
        /// </summary>
        public ToscaServiceTemplateAlreadyExistsException()
        {
        }

        /// <summary>
        /// Initializes an instance of <see cref="ToscaServiceTemplateAlreadyExistsException"/> with a message
        /// </summary>
        /// <param name="message"></param>
        public ToscaServiceTemplateAlreadyExistsException(string message) : base(message)
        {
        }

        /// <summary>
        /// Deserializes an instance of <see cref="ToscaServiceTemplateAlreadyExistsException"/>
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        /// <exception cref="ArgumentNullException">The <paramref name="info" /> parameter is null. </exception>
        /// <exception cref="SerializationException">The class name is null or <see cref="P:System.Exception.HResult" /> is zero (0). </exception>
        public ToscaServiceTemplateAlreadyExistsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        /// <summary>
        /// Initializes an instance of <see cref="ToscaServiceTemplateAlreadyExistsException"/> with a message and an inner exception
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public ToscaServiceTemplateAlreadyExistsException(string message, Exception exception) : base(message, exception)
        {
        }
    }
}