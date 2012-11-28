using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.Serialization;
using SevenDigital.Api.Wrapper.Http;

namespace SevenDigital.Api.Wrapper.Exceptions
{
	public abstract class ApiResponseException : ApiException
	{
		public HttpStatusCode StatusCode { get; private set; }
		public string ResponseBody { get; private set; }
		public IDictionary<string, string> Headers { get; private set; }

		protected ApiResponseException(string message, Response response)
			: base(message)
		{
			ResponseBody = response.Body;
			Headers = response.Headers;
			StatusCode = response.StatusCode;
		}

		protected ApiResponseException(string message, Exception innerException, Response response)
			: base(message, innerException)
		{
			ResponseBody = response.Body;
			Headers = response.Headers;
			StatusCode = response.StatusCode;
		}

		protected ApiResponseException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}