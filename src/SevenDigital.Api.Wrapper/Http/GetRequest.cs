using System;
using System.Collections.Generic;

namespace SevenDigital.Api.Wrapper.Http
{
	[Serializable]
	public class GetRequest
	{
		private readonly string _url;
		private readonly IDictionary<string, string> _headers;

		public GetRequest(string url, IDictionary<string, string> headers)
		{
			_url = url;
			_headers = headers;
		}

		public string Url
		{
			get { return _url; }
		}

		public IDictionary<string, string> Headers
		{
			get { return _headers; }
		}
	}
}