using System;

namespace SevenDigital.Api.Wrapper.Serialization.Exceptions
{
	internal class NonXmlContentException : SerializationException
	{
		const string DEFAULT_ERROR_MESSAGE = "Unexpected non-XML content found when deserializing";

		public NonXmlContentException(Exception innerException)
			: base(DEFAULT_ERROR_MESSAGE, innerException)
		{
		}
	}
}
