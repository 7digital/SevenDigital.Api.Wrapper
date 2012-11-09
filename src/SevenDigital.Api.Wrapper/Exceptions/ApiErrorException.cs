using System;
using System.Runtime.Serialization;
using SevenDigital.Api.Schema;
using SevenDigital.Api.Wrapper.Http;

namespace SevenDigital.Api.Wrapper.Exceptions
{
	public abstract class ApiErrorException : ApiException
	{
		public ErrorCode ErrorCode { get; private set; }

		protected ApiErrorException()
		{
		}

		protected ApiErrorException(string message, ErrorCode errorCode)
			: base(message)
		{
			ErrorCode = errorCode;
		}

		protected ApiErrorException(string message, Response response, ErrorCode errorCode)
			: base(message, response)
		{
			ErrorCode = errorCode;
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