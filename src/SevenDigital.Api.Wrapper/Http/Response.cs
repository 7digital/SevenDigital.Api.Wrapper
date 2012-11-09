using System;
using System.Net;
using System.Collections.Generic;

namespace SevenDigital.Api.Wrapper.Http
{
	[Serializable]
	public class Response
	{
		public HttpStatusCode StatusCode { get; private set; }
		public IDictionary<string, string> Headers { get; private set; }
		public string Body { get; private set; }

		public Response(HttpStatusCode statusCode, IDictionary<string, string> headers, string body)
		{
			StatusCode = statusCode;
			Headers = headers;
			Body = body;
		}

		public Response(HttpStatusCode statusCode, string body)
		{
			StatusCode = statusCode;
			Headers = new Dictionary<string, string>();
			Body = body;
		}

	}
}
