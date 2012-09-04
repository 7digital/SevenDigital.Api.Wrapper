using System;
using System.Collections.Generic;
using SevenDigital.Api.Wrapper.EndpointResolution.OAuth;
using SevenDigital.Api.Wrapper.Utility.Http;

namespace SevenDigital.Api.Wrapper.EndpointResolution.RequestHandlers
{
	public class PostRequestHandler : RequestHandler
	{
		private readonly IOAuthCredentials _oAuthCredentials;
		private readonly IUrlSigner _urlSigner;

		public PostRequestHandler(IApiUri apiUri, IOAuthCredentials oAuthCredentials, IUrlSigner urlSigner) : base(apiUri)
		{
			_oAuthCredentials = oAuthCredentials;
			_urlSigner = urlSigner;
		}

		public override Response HitEndpoint(EndPointInfo endPointInfo)
		{
			var postRequest = BuildPostRequest(endPointInfo);
			return HttpClient.Post(postRequest);
		}
		public override void HitEndpointAsync(EndPointInfo endPointInfo, Action<Response> action)
		{
			var postRequest = BuildPostRequest(endPointInfo);
			HttpClient.PostAsync(postRequest,response => action(response));
		}

		private Request BuildPostRequest(EndPointInfo endPointInfo)
		{
			var uri = ConstructEndpoint(endPointInfo);
			var signedParams = SignHttpPostParams(uri, endPointInfo);
			var postRequest = new Request(uri, endPointInfo.Headers, signedParams);
			return postRequest;
		}

		private IDictionary<string, string> SignHttpPostParams(string uri, EndPointInfo endPointInfo)
		{
			if (endPointInfo.IsSigned)
			{
				return _urlSigner.SignPostRequest(uri, endPointInfo.UserToken, endPointInfo.UserSecret, _oAuthCredentials, endPointInfo.Parameters);
			}
			return endPointInfo.Parameters;
		}

		protected override string AdditionalParameters(Dictionary<string, string> newDictionary)
		{
			return String.Empty;
		}
	}
}