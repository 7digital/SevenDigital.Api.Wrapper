
using System;
using System.Net;

namespace SevenDigital.Api.Wrapper.Exceptions
{
	[Serializable]
	public class ApiWebException : ApiException
	{
		public ApiWebException(string msg, string uri, WebException innerException) :
			base (msg, uri, innerException)
		{
		}
	}
}
