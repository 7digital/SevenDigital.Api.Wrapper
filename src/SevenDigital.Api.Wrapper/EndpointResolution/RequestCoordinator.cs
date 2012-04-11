using System;
using System.Linq;
using System.Text.RegularExpressions;
using SevenDigital.Api.Wrapper.EndpointResolution.OAuth;
using SevenDigital.Api.Wrapper.Utility.Http;
using System.Collections.Generic;

namespace SevenDigital.Api.Wrapper.EndpointResolution
{
	public class RequestCoordinator : IRequestCoordinator
	{
		private IHttpClient _httpClient;
		private readonly IUrlSigner _urlSigner;
		private readonly IOAuthCredentials _oAuthCredentials;
		private readonly IApiUri _apiUri;

		public RequestCoordinator(IHttpClient httpClient, IUrlSigner urlSigner, IOAuthCredentials oAuthCredentials, IApiUri apiUri)
		{
			_httpClient = httpClient;
			_urlSigner = urlSigner;
			_oAuthCredentials = oAuthCredentials;
			_apiUri = apiUri;
		}

		public IHttpClient HttpClient
		{
			get { return _httpClient; }
			set { _httpClient = value; }
		}

		public virtual string HitEndpoint(EndPointInfo endPointInfo)
		{
			return HitEndpointAndGetResponse(endPointInfo).Body;
		}

		public IResponse HitEndpointAndGetResponse(EndPointInfo endPointInfo)
		{
			var signedUrl = GetSignedUrl(endPointInfo);

			var request = new Request(signedUrl, endPointInfo.Headers, string.Empty);
			switch (endPointInfo.HttpMethod.ToUpperInvariant())
			{
				case "GET":
					return HttpClient.Get(request);
				case "POST":
					var data = endPointInfo.Parameters.ToQueryString();
					return HttpClient.Post(request);
				default:
					throw new NotImplementedException();
			}
		}

		public virtual void HitEndpointAsync(EndPointInfo endPointInfo, Action<string> payload)
		{
			var signedUrl = GetSignedUrl(endPointInfo);

			var request = new Request(signedUrl, endPointInfo.Headers, string.Empty);
			switch (endPointInfo.HttpMethod.ToUpperInvariant())
			{
				case "GET":
					HttpClient.GetAsync(request, (response) => payload(response.Body));
					break;
				case "POST":
					var data = endPointInfo.Parameters.ToQueryString();
					HttpClient.PostAsync(request, (response) => payload(response.Body));
					break;
				default:
					throw new NotImplementedException();
			}
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