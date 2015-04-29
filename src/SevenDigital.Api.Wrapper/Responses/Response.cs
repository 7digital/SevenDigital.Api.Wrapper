using System;
using System.Collections.Generic;
using System.Net;
using SevenDigital.Api.Wrapper.Requests;

namespace SevenDigital.Api.Wrapper.Responses
{
	[Serializable]
	public class Response
	{
		public HttpStatusCode StatusCode { get; private set; }
		public IDictionary<string, string> Headers { get; private set; }
		public string Body { get; private set; }
		public Request OriginalRequest { get; private set; }

		public Response(HttpStatusCode statusCode, IDictionary<string, string> headers, string body, Request originalRequest)
		{
			OriginalRequest = originalRequest;
			StatusCode = statusCode;
			Headers = new Dictionary<string, string>(headers, StringComparer.OrdinalIgnoreCase);
			Body = body;
		}

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

		public bool ContentTypeIsJson()
		{
			const string ContentTypeHeaderKey = "Content-Type";
			if (!Headers.ContainsKey(ContentTypeHeaderKey))
 			{
 				return false;
 			}

			var contentType = Headers[ContentTypeHeaderKey];
 			return contentType.StartsWith("application/json") || contentType.StartsWith("text/json");
 		}
	}
}
