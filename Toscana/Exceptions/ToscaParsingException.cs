using System;
using System.Runtime.Serialization;
using YamlDotNet.Core;

namespace Toscana.Exceptions
{
    [Serializable]
    public class ToscaParsingException : ToscaBaseException
    {
        public ToscaParsingException()
        {
        }

        public ToscaParsingException(string message) : base(message)
        {
        }

        public ToscaParsingException(string message, Exception exception) : base(message, exception)
        {
        }

        protected ToscaParsingException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}