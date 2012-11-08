using System;
using System.Runtime.Serialization;
using SevenDigital.Api.Wrapper.Utility.Http;

namespace SevenDigital.Api.Wrapper.Exceptions
{
	[Serializable]
	public class NonXmlResponseException : ApiException
	{
		public const string DEFAULT_ERROR_MESSAGE = "Error deserializing xml response";

		public NonXmlResponseException()
			: base(DEFAULT_ERROR_MESSAGE)
		{
		}

		public NonXmlResponseException(Response response)
			: base(DEFAULT_ERROR_MESSAGE, response)
		{
		}

		public NonXmlResponseException(string message)
			: base(message)
		{
		}

		public NonXmlResponseException(string message, Exception innerException, Response response)
			: base(message, innerException, response)
		{
		}

		protected NonXmlResponseException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}