using System;
using System.Runtime.Serialization;

namespace SevenDigital.Api.Wrapper.Exceptions
{
	[Serializable]
	public class UserCardException : ApiErrorException
	{
		public UserCardException()
			: base()
		{
		}

		public UserCardException(string message)
			: base(message)
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
