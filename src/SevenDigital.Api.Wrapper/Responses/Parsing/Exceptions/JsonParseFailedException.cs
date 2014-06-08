using System;
using System.Runtime.Serialization;

namespace SevenDigital.Api.Wrapper.Responses.Parsing.Exceptions
{
	[Serializable]
	internal class JsonParseFailedException : SerializationException
	{
		const string DEFAULT_ERROR_MESSAGE = "Json parse failed";

		public JsonParseFailedException()
			: base(DEFAULT_ERROR_MESSAGE)
		{
		}

		public JsonParseFailedException(Exception innerException)
			: base(DEFAULT_ERROR_MESSAGE, innerException)
		{
		}

		protected JsonParseFailedException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}