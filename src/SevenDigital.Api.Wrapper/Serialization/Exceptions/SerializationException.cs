using System;
using System.Runtime.Serialization;

namespace SevenDigital.Api.Wrapper.Serialization.Exceptions
{
	internal abstract class SerializationException : Exception
	{
		internal SerializationException(string msg, Exception innerException)
			: base(msg, innerException)
		{
		}

		protected SerializationException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
