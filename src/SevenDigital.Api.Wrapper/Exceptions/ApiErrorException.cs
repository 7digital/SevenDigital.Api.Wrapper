
using System.Runtime.Serialization;
using SevenDigital.Api.Schema;
using SevenDigital.Api.Wrapper.Http;

namespace SevenDigital.Api.Wrapper.Exceptions
{
	public abstract class ApiErrorException : ApiResponseException
	{
		public ErrorCode ErrorCode { get; private set; }

		protected ApiErrorException(string message, Response response, ErrorCode errorCode)
			: base(message, response)
		{
			ErrorCode = errorCode;
		}

		protected ApiErrorException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
