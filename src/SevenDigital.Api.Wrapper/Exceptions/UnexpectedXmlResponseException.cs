using System;
using SevenDigital.Api.Wrapper.Http;

namespace SevenDigital.Api.Wrapper.Exceptions
{
	public class UnexpectedXmlResponseException : ApiException
	{
		const string DEFAULT_ERROR_MESSAGE = "Error deserializing XML content in API response";

		public UnexpectedXmlResponseException(Exception inner, Response response): base(DEFAULT_ERROR_MESSAGE, inner, response)
		{
		}
	}
}
