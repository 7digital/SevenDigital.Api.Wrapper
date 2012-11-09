using System;
using System.Runtime.Serialization;
using SevenDigital.Api.Schema;
using SevenDigital.Api.Wrapper.Utility.Http;

namespace SevenDigital.Api.Wrapper.Exceptions
{
	public abstract class ApiErrorException : ApiException
	{
		public int ErrorCode { get; private set; }

		protected ApiErrorException()
		{
		}

		protected ApiErrorException(string message, Response response, Error error)
			: base(message, response)
		{
			ErrorCode = error.Code;
		}

		protected ApiErrorException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		protected ApiErrorException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}