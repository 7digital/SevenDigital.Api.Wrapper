using System;
using System.Runtime.Serialization;

namespace SevenDigital.Api.Wrapper.Exceptions
{
	[Serializable]
	public class InvalidResourceException : ApiErrorException
	{
		public InvalidResourceException()
			: base()
		{
		}

		public InvalidResourceException(string message)
			: base(message)
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
