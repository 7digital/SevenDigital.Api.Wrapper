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

		public override Response HitEndpoint(RequestData requestData)
		{
			var getRequest = BuildGetRequest(requestData);
			return HttpClient.Get(getRequest);
		}

		private GetRequest BuildGetRequest(RequestData requestData)
		{
			var uri = ConstructEndpoint(requestData);
			var signedUrl = SignHttpGetUrl(uri, requestData);
			var getRequest = new GetRequest(signedUrl, requestData.Headers);
			return getRequest;
		}

		public override void HitEndpointAsync(RequestData requestData, Action<Response> action)
		{
			var getRequest = BuildGetRequest(requestData);
			HttpClient.GetAsync(getRequest, response => action(response));
		}

		private string SignHttpGetUrl(string uri, RequestData requestData)
		{
			if (requestData.IsSigned)
			{
				return _urlSigner.SignGetUrl(uri, requestData.UserToken, requestData.TokenSecret, _oAuthCredentials);
			}
			return uri;
		}

		protected override string AdditionalParameters(Dictionary<string, string> newDictionary)
		{
			return string.Format("?oauth_consumer_key={0}&{1}", _oAuthCredentials.ConsumerKey, newDictionary.ToQueryString(true)).TrimEnd('&');
		}
	}
}