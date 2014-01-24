using System.Collections.Generic;
using SevenDigital.Api.Wrapper.Http;

namespace SevenDigital.Api.Wrapper.EndpointResolution.OAuth
{
	public class OAuthHeaderData
	{
		private IDictionary<string, string> _requestParameters = new Dictionary<string, string>();

		public string Url { get; set; }
		public HttpMethod HttpMethod { get; set; }
		public string UserToken { get; set; }
		public string TokenSecret { get; set; }

		public IDictionary<string, string> RequestParameters
		{
			get { return _requestParameters; }
			set { _requestParameters = value; }
		}
	}
}