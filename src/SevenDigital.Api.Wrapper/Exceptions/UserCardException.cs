using System;
using System.Runtime.Serialization;
using SevenDigital.Api.Schema;
using SevenDigital.Api.Wrapper.Http;

namespace SevenDigital.Api.Wrapper.Exceptions
{
	[Serializable]
	public class UserCardException : ApiErrorException
	{
		public UserCardException()
			: base()
		{
		}

		public UserCardException(string message, Response response, Error error)
			: base(message, response, error)
		{
		}

		public UserCardException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		protected UserCardException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
