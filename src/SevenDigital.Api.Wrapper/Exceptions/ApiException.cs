using System;
using System.Net;
using System.Runtime.Serialization;
using SevenDigital.Api.Wrapper.Utility.Http;

namespace SevenDigital.Api.Wrapper.Exceptions
{
	public abstract class ApiException : Exception
	{
		public string Uri { get; internal set; }
		public HttpStatusCode StatusCode { get; private set; }
		public string ResponseBody { get; private set; }

		protected ApiException()
		{
		}

		protected ApiException(string message)
			: base(message)
		{
		}

		protected ApiException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		protected ApiException(string message, Exception innerException, Response response)
			: base(message, innerException)
		{
			ResponseBody = response.Body;
			StatusCode = response.StatusCode;
		}

		protected ApiException(string message, Response response)
			: base(message)
		{
			ResponseBody = response.Body;
			StatusCode = response.StatusCode;
		}

		protected ApiException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}