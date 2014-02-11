using System;
using System.Runtime.Serialization;
using SevenDigital.Api.Schema;
using SevenDigital.Api.Wrapper.Responses;

namespace SevenDigital.Api.Wrapper.Exceptions
{
	[Serializable]
	public class RemoteApiException : ApiErrorException
	{
		public RemoteApiException(string message, Response response, ErrorCode errorCode)
			: base(message, response, errorCode)
		{
		}

		protected RemoteApiException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
