using System.Collections.Generic;

namespace SevenDigital.Api.Wrapper.Utility.Http
{
	public class Request : IRequest
	{
		private readonly string _url;
		private readonly Dictionary<string, string> _headers;

		public Request(string url, Dictionary<string, string> headers)
		{
			_url = url;
			_headers = headers;
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