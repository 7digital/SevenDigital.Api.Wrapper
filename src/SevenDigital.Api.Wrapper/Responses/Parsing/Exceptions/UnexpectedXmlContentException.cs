using System;
using System.Runtime.Serialization;

namespace SevenDigital.Api.Wrapper.Responses.Parsing.Exceptions
{
	[Serializable]
	internal class UnexpectedXmlContentException : SerializationException
	{
		private const string DEFAULT_ERROR_MESSAGE = "Unexpected content found when deserializing XML";

		internal UnexpectedXmlContentException(Exception innerException) : base (DEFAULT_ERROR_MESSAGE, innerException)
		{
		}

		protected UnexpectedXmlContentException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
