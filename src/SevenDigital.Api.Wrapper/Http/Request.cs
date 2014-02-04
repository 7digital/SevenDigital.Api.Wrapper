using System;
using System.Collections.Generic;

namespace SevenDigital.Api.Wrapper.Http
{
	[Serializable]
	public class Request
	{
		public Request(HttpMethod method, string url, IDictionary<string, string> headers, string body)
		{
			Method = method;
			Url = url;
			Headers = headers;
			Body = body;
		}

		public HttpMethod Method { get; private set; }
		public string Url { get; private set; }
		public IDictionary<string, string> Headers { get; private set; }
		public string Body { get; private set; }
	}
}