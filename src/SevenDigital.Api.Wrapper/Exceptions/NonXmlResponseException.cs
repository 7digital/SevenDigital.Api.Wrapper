using System;
using System.Runtime.Serialization;
using SevenDigital.Api.Wrapper.Http;

namespace SevenDigital.Api.Wrapper.Exceptions
{
	[Serializable]
	public class NonXmlResponseException : ApiResponseException
	{
		public const string DEFAULT_ERROR_MESSAGE = "Response is not xml";

		public NonXmlResponseException(Response response)
			: base(DEFAULT_ERROR_MESSAGE, response)
		{
		}

		public NonXmlResponseException(Exception innerException, Response response)
			: base(DEFAULT_ERROR_MESSAGE, innerException, response)
		{
		}

		protected NonXmlResponseException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}