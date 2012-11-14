using System;
using System.Runtime.Serialization;
using SevenDigital.Api.Schema;
using SevenDigital.Api.Wrapper.Http;

namespace SevenDigital.Api.Wrapper.Exceptions
{
	[Serializable]
	public class InvalidResourceException : ApiErrorException
	{
		public InvalidResourceException()
			: base()
		{
		}

		public InvalidResourceException(string message, ErrorCode errorCode)
			: base(message, errorCode)
		{
		}

		public InvalidResourceException(string message, Response response, ErrorCode errorCode)
			: base(message, response, errorCode)
		{
		}

		protected InvalidResourceException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
