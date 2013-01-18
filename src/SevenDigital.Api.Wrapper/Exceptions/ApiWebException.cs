using System;
using System.Net;
using System.Runtime.Serialization;

namespace SevenDigital.Api.Wrapper.Exceptions
{
	[Serializable]
	public class ApiWebException : ApiException
	{
		public ApiWebException(string msg, string uri, WebException innerException) :
			base(msg, uri, innerException)
		{
		}

		protected ApiWebException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
