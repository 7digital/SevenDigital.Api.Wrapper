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

		public InvalidResourceException(string message, Response response, Error error)
			: base(message, response, error)
		{
		}

		public InvalidResourceException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		protected InvalidResourceException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
