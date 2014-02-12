using System;
using System.Collections.Generic;
using SevenDigital.Api.Wrapper.Http;

namespace SevenDigital.Api.Wrapper.Requests
{
	public class RequestData
	{
		public string Endpoint { get; set; }

		public HttpMethod HttpMethod { get; set; }

		public IDictionary<string,string> Parameters { get; set; }

		public IDictionary<string,string> Headers { get; set; }

		public bool UseHttps { get; set; }

		public string UserToken { get; set; }

		public string TokenSecret { get; set; }

		[Obsolete("Use TokenSecret")]
		public string UserSecret { get { return TokenSecret; } set { TokenSecret = value; } }

		public bool RequiresSignature { get; set; }

		public bool HasToken
		{
			get { return UserToken != null; }
		}

		public RequestData()
		{
			Endpoint = string.Empty;
			HttpMethod = HttpMethod.Get;
			Parameters = new Dictionary<string,string>();
			Headers = new Dictionary<string,string>();
			UseHttps = false;
		}
	}
}