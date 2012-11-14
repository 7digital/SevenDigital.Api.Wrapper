
using System;

namespace SevenDigital.Api.Wrapper.Serialization.Exceptions
{
	internal class UnexpectedXmlContentException : SerializationException
	{
		private const string DEFAULT_ERROR_MESSAGE = "Unexpected content found when deserializing XML";

		internal UnexpectedXmlContentException(Exception innerException) : base (DEFAULT_ERROR_MESSAGE, innerException)
		{
		}
	}
}
