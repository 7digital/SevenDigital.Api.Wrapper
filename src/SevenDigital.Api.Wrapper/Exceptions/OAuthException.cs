using System;
using System.Runtime.Serialization;
using SevenDigital.Api.Wrapper.Http;

namespace SevenDigital.Api.Wrapper.Exceptions
{
	[Serializable]
	public class OAuthException : ApiException
	{
		public OAuthException()
			: base()
		{
		}

		public OAuthException(string message)
			: base(message)
		{
		}

		public OAuthException(Response response) : base (response.Body, response)
		{
		}

		public OAuthException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		protected OAuthException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}