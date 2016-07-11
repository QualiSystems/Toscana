using System;
using System.Runtime.Serialization;

namespace Toscana.Exceptions
{
    /// <summary>
    /// Thrown when Capability type is not found
    /// </summary>
    [Serializable]
    public class ToscaCapabilityTypeNotFoundException : ToscaBaseException
    {
        /// <summary>
        /// Ctor without any message
        /// </summary>
        public ToscaCapabilityTypeNotFoundException()
        {
        }

        /// <summary>
        /// Ctor with a message
        /// </summary>
        /// <param name="message"></param>
        public ToscaCapabilityTypeNotFoundException(string message) : base(message)
        {
        }

        /// <summary>
        /// Ctor to be used for serialization
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected ToscaCapabilityTypeNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}