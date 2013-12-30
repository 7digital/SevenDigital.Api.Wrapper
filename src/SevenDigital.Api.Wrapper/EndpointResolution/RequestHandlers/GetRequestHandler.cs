using System;
using System.Collections.Generic;
using SevenDigital.Api.Schema.OAuth;
using SevenDigital.Api.Wrapper.EndpointResolution.OAuth;
using SevenDigital.Api.Wrapper.Http;

namespace SevenDigital.Api.Wrapper.EndpointResolution.RequestHandlers
{
	public class GetRequestHandler : RequestHandler
	{
		private readonly IOAuthCredentials _oAuthCredentials;
		private readonly ISignatureGenerator _signatureGenerator;

		public GetRequestHandler(IApiUri apiUri, IOAuthCredentials oAuthCredentials, ISignatureGenerator signatureGenerator) : base(apiUri)
		{
			_oAuthCredentials = oAuthCredentials;
			_signatureGenerator = signatureGenerator;
		}

		public override bool HandlesMethod(string method)
		{
			return method == "GET";
		}

		public override Response HitEndpoint(RequestData requestData)
		{
			var getRequest = BuildGetRequest(requestData);
			return HttpClient.Get(getRequest);
		}

		public override void HitEndpointAsync(RequestData requestData, Action<Response> action)
		{
			var getRequest = BuildGetRequest(requestData);
			HttpClient.GetAsync(getRequest, response => action(response));
		}

		private GetRequest BuildGetRequest(RequestData requestData)
		{
			var uri = ConstructEndpoint(requestData);
			var signedUrl = SignHttpGetUrl(uri, requestData);
			var getRequest = new GetRequest(signedUrl, requestData.Headers);
			return getRequest;
		}

		private string SignHttpGetUrl(string uri, RequestData requestData)
		{
			if (!requestData.IsSigned)
			{
				return uri;
			}
			
			var oAuthSignatureInfo = new OAuthSignatureInfo
			{
				FullUrlToSign = uri,
				ConsumerCredentials = _oAuthCredentials,
				HttpMethod = "GET",
				UserAccessToken = new OAuthAccessToken { Token = requestData.UserToken, Secret = requestData.TokenSecret }
			};
			return _signatureGenerator.Sign(oAuthSignatureInfo);
		}

		protected override string AdditionalParameters(Dictionary<string, string> newDictionary)
		{
			return string.Format("?oauth_consumer_key={0}&{1}", _oAuthCredentials.ConsumerKey, newDictionary.ToQueryString(true)).TrimEnd('&');
		}
	}
}