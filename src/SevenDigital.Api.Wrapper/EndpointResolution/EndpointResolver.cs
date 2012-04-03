using System;
using System.Linq;
using System.Text.RegularExpressions;
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
			var signedUrl = GetSignedUrl(endPointInfo);
			return _urlResolver.Resolve(signedUrl, endPointInfo.HttpMethod, new Dictionary<string, string>());
		}

		public virtual void HitEndpointAsync(EndPointInfo endPointInfo, Action<string> payload)
		{
			var signedUrl = GetSignedUrl(endPointInfo);
			_urlResolver.ResolveAsync(signedUrl, endPointInfo.HttpMethod, new Dictionary<string, string>(), payload);
		}

		public string ConstructEndpoint(EndPointInfo endPointInfo)
		{
			return GetSignedUrl(endPointInfo);
		}

		private string GetSignedUrl(EndPointInfo endPointInfo)
		{
			string apiUri = _apiUri.Uri;
			var newDictionary = endPointInfo.Parameters.ToDictionary(entry => entry.Key, entry => entry.Value);
			if (endPointInfo.UseHttps)
				apiUri = apiUri.Replace("http://", "https://");

			var uriString = string.Format("{0}/{1}?oauth_consumer_key={2}&{3}",
				apiUri, SubstituteRouteParameters(endPointInfo.Uri, newDictionary),
				_oAuthCredentials.ConsumerKey,
				newDictionary.ToQueryString(true)).TrimEnd('&');

			if (endPointInfo.IsSigned)
				uriString = _urlSigner.SignUrlAsString(uriString, endPointInfo.UserToken, endPointInfo.UserSecret, _oAuthCredentials);

			return uriString;
		}

		private static string SubstituteRouteParameters(string endpointUri, Dictionary<string, string> parameters)
		{
			
			var regex = new Regex("{(.*?)}");
			var result = regex.Matches(endpointUri);
			foreach (var match in result)
			{
				var key = match.ToString().Remove(match.ToString().Length - 1).Remove(0, 1);
				var entry = parameters.First(x => x.Key.Equals(key, StringComparison.InvariantCultureIgnoreCase));
				parameters.Remove(entry.Key);
				endpointUri = endpointUri.Replace(match.ToString(), entry.Value);
			}

			return endpointUri.ToLower();
		}
	}
}