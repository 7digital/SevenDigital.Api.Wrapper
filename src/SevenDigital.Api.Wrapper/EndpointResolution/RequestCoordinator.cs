using System;
using SevenDigital.Api.Wrapper.EndpointResolution.OAuth;
using SevenDigital.Api.Wrapper.EndpointResolution.RequestHandlers;
using SevenDigital.Api.Wrapper.Utility.Http;

namespace SevenDigital.Api.Wrapper.EndpointResolution
{
	public class RequestCoordinator : IRequestCoordinator
	{
		private readonly IUrlSigner _urlSigner;
		private readonly IOAuthCredentials _oAuthCredentials;
		private readonly IApiUri _apiUri;
		
		public IHttpClient HttpClient { get; set; }

		public RequestCoordinator(IHttpClient httpClient, IUrlSigner urlSigner, IOAuthCredentials oAuthCredentials, IApiUri apiUri)
		{
			HttpClient = httpClient;
			_urlSigner = urlSigner;
			_oAuthCredentials = oAuthCredentials;
			_apiUri = apiUri;
		}

		public string ConstructEndpoint(EndPointInfo endPointInfo)
		{
			return ConstructBuilder(endPointInfo).ConstructEndpoint(endPointInfo);
		}

		private RequestHandler ConstructBuilder(EndPointInfo endPointInfo)
		{
			switch (endPointInfo.HttpMethod.ToUpperInvariant())
			{
				case "GET":
					return new GetRequestHandler(_apiUri, _oAuthCredentials, _urlSigner);
				case "POST":
					return new PostRequestHandler(_apiUri, _oAuthCredentials, _urlSigner);
				default:
					throw new NotImplementedException();
			}
		}


		public virtual string HitEndpoint(EndPointInfo endPointInfo)
		{
			var builder = ConstructBuilder(endPointInfo);
			builder.HttpClient = HttpClient;
			return builder.HitEndpoint(endPointInfo);
		}


		public virtual void HitEndpointAsync(EndPointInfo endPointInfo, Action<string> callback)
		{
			var builder = ConstructBuilder(endPointInfo);
			builder.HttpClient = HttpClient;
			builder.HitEndpointAsync(endPointInfo, callback);
		}
	}
}