using System;
using System.Collections.Generic;

namespace SevenDigital.Api.Wrapper.Http
{
	[Serializable]
	public class PostRequest
	{
		private readonly string _url;
		private readonly IDictionary<string, string> _headers;
		private readonly string _body;

		public PostRequest(string url, IDictionary<string, string> headers, string body)
		{
			_url = url;
			_headers = headers;
			_body = body;
		}

		public string Url
		{
			get { return _url; }
		}

		public IDictionary<string, string> Headers
		{
			get { return _headers; }
		}

		public string Body
		{
			get { return _body; }
		}
	}
}