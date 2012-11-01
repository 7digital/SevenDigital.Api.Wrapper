using System;
using System.Runtime.Serialization;

namespace SevenDigital.Api.Wrapper.Exceptions
{
	[Serializable]
	public class OAuthException : ApiException
	{
		public const string DEFAULT_ERROR_MESSAGE = "Error deserializing xml response";

		public OAuthException()
			: base(DEFAULT_ERROR_MESSAGE)
		{
		}

		public OAuthException(string message)
			: base(message)
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