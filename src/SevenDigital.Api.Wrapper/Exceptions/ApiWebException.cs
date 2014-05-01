using System;
using System.Net;
using System.Runtime.Serialization;
using SevenDigital.Api.Wrapper.Requests;

namespace SevenDigital.Api.Wrapper.Exceptions
{
	[Serializable]
	public class ApiWebException : ApiException
	{
		public ApiWebException(string msg, WebException innerException) :
			base(msg,  innerException)
		{
		}

		public ApiWebException(string msg, WebException innerException, Request originalRequest) :
			base(msg, innerException, originalRequest)
		{
		}

		protected ApiWebException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
