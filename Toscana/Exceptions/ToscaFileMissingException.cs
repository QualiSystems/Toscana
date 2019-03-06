using System;
using System.Runtime.Serialization;

namespace Toscana.Exceptions
{

	/// <summary>
	/// Thrown when TOSCA Cloud Service Archive (CSAR) not found
	/// </summary>
	[Serializable]
	public class ToscaFileMissingException : ToscaBaseException
	{
		
		/// <summary>
		/// Missing file name
		/// </summary>
		public string MissingFileName { get; set; }

		/// <summary>
		/// Initializes the exception without any message
		/// </summary>
		public ToscaFileMissingException()
		{
		}

		/// <summary>
		/// Initializes the exception with specified message
		/// </summary>
		/// <param name="message"></param>
		public ToscaFileMissingException(string message) : base(message)
		{
		}

		/// <summary>
		/// Initializes the exception with a message and an inner exception
		/// </summary>
		/// <param name="message">Exception message</param>
		/// <param name="exception">Inner exception</param>
		public ToscaFileMissingException(string message, Exception exception) : base(message, exception)
		{
		}

		/// <summary>
		/// Initializes the exception with specified message
		/// </summary>		
		/// <param name="message"></param>
		/// <param name="missingFileNameName"></param>
		public ToscaFileMissingException(string message, string missingFileNameName) : base(message)
		{
			MissingFileName = missingFileNameName;
		}

		/// <summary>
		/// Initializes the exception based on serialization info. Used implicitly by when an exception is deserialized.
		/// </summary>
		/// <param name="info">Serialization info</param>
		/// <param name="context">Streaming context</param>
		/// <exception cref="ArgumentNullException">The <paramref name="info" /> parameter is null. </exception>
		/// <exception cref="SerializationException">The class name is null or <see cref="P:System.Exception.HResult" /> is zero (0). </exception>
		public ToscaFileMissingException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}		
	}
}
