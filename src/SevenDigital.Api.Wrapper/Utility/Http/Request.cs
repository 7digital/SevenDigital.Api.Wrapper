using System;
using System.Collections.Generic;

namespace SevenDigital.Api.Wrapper.Utility.Http
{
	[Serializable]
	public class Request
	{
		private readonly string _url;
		private readonly IDictionary<string, string> _headers;
		private readonly IDictionary<string, string> _parameters;

		public Request(string url, IDictionary<string, string> headers)
		{
			_url = url;
			_headers = headers;
		}

		public Request(string url, Dictionary<string, string> headers, IDictionary<string, string> parameters)
		{
			_url = url;
			_headers = headers;
			_parameters = parameters;
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