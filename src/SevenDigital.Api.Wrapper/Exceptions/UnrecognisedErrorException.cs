using System;
using System.Runtime.Serialization;
using SevenDigital.Api.Wrapper.Http;

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

		public UnrecognisedErrorException(Exception innerException, Response response)
			: base(DEFAULT_ERROR_MESSAGE, innerException, response)
		{
		}

		public UnrecognisedErrorException(string message, Response response)
			: base(message, response)
		{
		}

		public UnrecognisedErrorException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		public UnrecognisedErrorException(string message, Exception innerException, Response response)
			: base(message, innerException, response)
		{
		}

		protected UnrecognisedErrorException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}