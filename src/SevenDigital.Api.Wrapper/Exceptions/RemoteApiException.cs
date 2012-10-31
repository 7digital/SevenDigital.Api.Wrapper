using System;
using System.Net;
using System.Runtime.Serialization;

namespace SevenDigital.Api.Wrapper.Exceptions
{
	[Serializable]
	public class RemoteApiException : ApiErrorException
	{
		public RemoteApiException()
			: base()
		{
		}

		public RemoteApiException(string message)
			: base(message)
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
