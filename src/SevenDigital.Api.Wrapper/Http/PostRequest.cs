using System;
using System.Collections.Generic;

namespace SevenDigital.Api.Wrapper.Http
{
	[Serializable]
	public class PostRequest
	{
		private readonly string _url;
		private readonly IDictionary<string, string> _headers;
		private readonly IDictionary<string, string> _parameters;

		public PostRequest(string url, IDictionary<string, string> headers, IDictionary<string, string> parameters)
		{
			_url = url;
			_headers = headers;
			_parameters = parameters;
		}

		public string Url
		{
			get { return _url; }
		}

		public IDictionary<string, string> Headers
		{
			get { return _headers; }
		}

		public IDictionary<string, string> Parameters
		{
			get { return _parameters; }
		}
	}
}