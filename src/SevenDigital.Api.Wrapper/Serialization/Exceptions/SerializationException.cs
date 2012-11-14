using System;

namespace SevenDigital.Api.Wrapper.Serialization.Exceptions
{
	internal abstract class SerializationException : Exception
	{
		internal SerializationException(string msg, Exception innerException)
			: base(msg, innerException)
		{
		}
	}
}
