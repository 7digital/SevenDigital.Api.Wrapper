using System;
using System.Runtime.Serialization;
using SevenDigital.Api.Wrapper.Responses;

namespace SevenDigital.Api.Wrapper.Exceptions
{
	[Serializable]
	public class OAuthException : ApiResponseException
	{
		public OAuthException(Response response) 
			: base (response.Body, response)
		{
		}

		protected OAuthException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}