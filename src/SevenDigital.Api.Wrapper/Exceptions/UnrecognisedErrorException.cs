using System;
using System.Runtime.Serialization;

namespace SevenDigital.Api.Wrapper.Exceptions
{
	[Serializable]
	public class UnrecognisedErrorException : ApiException
	{
		public const string DEFAULT_ERROR_MESSAGE = "Error parsing error XML";

		public UnrecognisedErrorException()
			: base(DEFAULT_ERROR_MESSAGE)
		{
		}

		public UnrecognisedErrorException(string message)
			: base(message)
		{
		}

		public UnrecognisedErrorException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		protected UnrecognisedErrorException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}