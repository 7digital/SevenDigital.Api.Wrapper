using System;
using System.Runtime.Serialization;
using SevenDigital.Api.Wrapper.Http;

namespace SevenDigital.Api.Wrapper.Exceptions
{
	[Serializable]
	public class UnexpectedXmlResponseException : ApiException
	{
		const string DEFAULT_ERROR_MESSAGE = "Error deserializing XML content in API response";

		public UnexpectedXmlResponseException(Exception inner, Response response): base(DEFAULT_ERROR_MESSAGE, inner, response)
		{
		}

		protected UnexpectedXmlResponseException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
