using System;
using System.Collections.Generic;
using SevenDigital.Api.Wrapper.EndpointResolution.OAuth;
using SevenDigital.Api.Wrapper.Http;

namespace SevenDigital.Api.Wrapper.EndpointResolution.RequestHandlers
{
	public class GetRequestHandler : RequestHandler
	{
		private readonly IOAuthCredentials _oAuthCredentials;
		private readonly IUrlSigner _urlSigner;

		public GetRequestHandler(IApiUri apiUri, IOAuthCredentials oAuthCredentials, IUrlSigner urlSigner) : base(apiUri)
		{
			_oAuthCredentials = oAuthCredentials;
			_urlSigner = urlSigner;
		}

		public override Response HitEndpoint(EndPointInfo endPointInfo)
		{
			var getRequest = BuildGetRequest(endPointInfo);
			return HttpClient.Get(getRequest);
		}

		private GetRequest BuildGetRequest(EndPointInfo endPointInfo)
		{
			var uri = ConstructEndpoint(endPointInfo);
			var signedUrl = SignHttpGetUrl(uri, endPointInfo);
			var getRequest = new GetRequest(signedUrl, endPointInfo.Headers);
			return getRequest;
		}

		public override void HitEndpointAsync(EndPointInfo endPointInfo, Action<Response> action)
		{
			var getRequest = BuildGetRequest(endPointInfo);
			HttpClient.GetAsync(getRequest, response => action(response));
		}

		private string SignHttpGetUrl(string uri, EndPointInfo endPointInfo)
		{
			if (endPointInfo.IsSigned)
			{
				return _urlSigner.SignGetUrl(uri, endPointInfo.UserToken, endPointInfo.TokenSecret, _oAuthCredentials);
			}
			return uri;
		}

		protected override string AdditionalParameters(Dictionary<string, string> newDictionary)
		{
			return string.Format("?oauth_consumer_key={0}&{1}", _oAuthCredentials.ConsumerKey, newDictionary.ToQueryString(true)).TrimEnd('&');
		}
	}
}