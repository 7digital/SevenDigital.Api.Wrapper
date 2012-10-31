using System;
using System.Runtime.Serialization;

namespace SevenDigital.Api.Wrapper.Exceptions
{
	public abstract class ApiErrorException : ApiException
	{
		public int ErrorCode { get; set; }

		protected ApiErrorException()
		{
		}

		protected ApiErrorException(string message)
			: base(message)
		{
		}

		protected ApiErrorException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		protected ApiErrorException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}