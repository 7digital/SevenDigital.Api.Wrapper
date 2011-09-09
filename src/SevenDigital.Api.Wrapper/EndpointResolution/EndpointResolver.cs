using System;
using SevenDigital.Api.Wrapper.EndpointResolution.OAuth;
using SevenDigital.Api.Wrapper.Utility.Http;
using System.Collections.Generic;

namespace SevenDigital.Api.Wrapper.EndpointResolution
{
	public class EndpointResolver : IEndpointResolver
	{
		private readonly IUrlResolver _urlResolver;
		private readonly IUrlSigner _urlSigner;
		private readonly IOAuthCredentials _oAuthCredentials;
		private readonly IApiUri _apiUri;

		public EndpointResolver(IUrlResolver urlResolver, IUrlSigner urlSigner, IOAuthCredentials oAuthCredentials, IApiUri apiUri)
		{
			_urlResolver = urlResolver;
			_urlSigner = urlSigner;
			_oAuthCredentials = oAuthCredentials;
			_apiUri = apiUri;
		}

		public virtual string HitEndpoint(EndPointInfo endPointInfo)
		{
			Uri signedUrl = GetSignedUrl(endPointInfo);

			return _urlResolver.Resolve(signedUrl, endPointInfo.HttpMethod, new Dictionary<string, string>());
		}

		public virtual void HitEndpointAsync(EndPointInfo endPointInfo, Action<string> payload)
		{
			Uri signedUrl = GetSignedUrl(endPointInfo);
			_urlResolver.ResolveAsync(signedUrl, endPointInfo.HttpMethod, new Dictionary<string, string>(), payload);
		}
		private Uri GetSignedUrl(EndPointInfo endPointInfo)
		{
			string apiUri = _apiUri.Uri;

			if (endPointInfo.UseHttps)
				apiUri = apiUri.Replace("http://", "https://");

			var uriString = string.Format("{0}/{1}?oauth_consumer_key={2}&{3}",
				apiUri, endPointInfo.Uri,
				_oAuthCredentials.ConsumerKey,
				endPointInfo.Parameters.ToQueryString()).TrimEnd('&');

			var signedUrl = new Uri(uriString);

			if (endPointInfo.IsSigned)
				signedUrl = _urlSigner.SignUrl(uriString, endPointInfo.UserToken, endPointInfo.UserSecret, _oAuthCredentials);
			return signedUrl;
		}
	}
}