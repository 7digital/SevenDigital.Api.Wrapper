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

		public RemoteApiException(string message, Response response, Error error)
			: base(message, response, error)
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
