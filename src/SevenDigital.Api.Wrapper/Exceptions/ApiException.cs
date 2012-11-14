using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.Serialization;
using SevenDigital.Api.Wrapper.Http;

namespace SevenDigital.Api.Wrapper.Exceptions
{
	public abstract class ApiException : Exception
	{
		public string Uri { get; internal set; }
		public HttpStatusCode StatusCode { get; private set; }
		public string ResponseBody { get; private set; }
		public IDictionary<string, string> Headers { get; private set; }

		protected ApiException()
		{
		}

		protected ApiException(string message)
			: base(message)
		{
		}

		protected ApiException(string message, Exception innerException, Response response)
			: base(message, innerException)
		{
			ResponseBody = response.Body;
			Headers = response.Headers;
			StatusCode = response.StatusCode;
		}

		protected ApiException(string message, Response response)
			: base(message)
		{
			ResponseBody = response.Body;
			Headers = response.Headers;
			StatusCode = response.StatusCode;
		}

		protected ApiException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}