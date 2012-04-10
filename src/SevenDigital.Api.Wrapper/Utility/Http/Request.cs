using System;
using System.Collections.Generic;

namespace SevenDigital.Api.Wrapper.Utility.Http
{
	[Serializable]
	public class Request : IRequest
	{
		private readonly string _url;
		private readonly Dictionary<string, string> _headers;

		public Request(string url, Dictionary<string, string> headers)
		{
			_url = url;
			_headers = headers;
		}

		public Request()
		{
			_url = string.Empty;
			_headers = new Dictionary<string, string>();
		}

		public string Url
		{
			get { return _url; }
		}

		public Dictionary<string, string> Headers
		{
			get { return _headers; }
		}
	}
}