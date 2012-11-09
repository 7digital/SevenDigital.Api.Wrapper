using System;
using System.Runtime.Serialization;
using SevenDigital.Api.Schema;
using SevenDigital.Api.Wrapper.Http;

namespace SevenDigital.Api.Wrapper.Exceptions
{
	[Serializable]
	public class RemoteApiException : ApiErrorException
	{
		public RemoteApiException()
			: base()
		{
		}

		public RemoteApiException(string message, ErrorCode errorCode)
			: base(message, errorCode)
		{
		}

		public RemoteApiException(string message, Response response, ErrorCode errorCode)
			: base(message, response, errorCode)
		{
		}

		public RemoteApiException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		protected RemoteApiException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
