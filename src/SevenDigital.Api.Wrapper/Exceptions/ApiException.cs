using System;
using System.Runtime.Serialization;

namespace SevenDigital.Api.Wrapper.Exceptions
{
	public abstract class ApiException : Exception
	{
		protected ApiException()
		{
		}

		protected ApiException(string message)
			: base(message)
		{
		}

		protected ApiException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		protected ApiException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}