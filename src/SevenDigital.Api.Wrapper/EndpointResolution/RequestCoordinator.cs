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

		public string ConstructEndpoint(EndPointInfo endPointInfo)
		{
			string apiUri = endPointInfo.UseHttps ? _apiUri.SecureUri : _apiUri.Uri;

			var newDictionary = endPointInfo.Parameters.ToDictionary(entry => entry.Key, entry => entry.Value);

			var uriString = string.Format("{0}/{1}", apiUri, SubstituteRouteParameters(endPointInfo.UriPath, newDictionary));

			if (endPointInfo.HttpMethod == "GET")
			{
				uriString = string.Format("{0}?oauth_consumer_key={1}&{2}", uriString, _oAuthCredentials.ConsumerKey, newDictionary.ToQueryString(true)).TrimEnd('&');
			}
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


		public virtual string HitEndpoint(EndPointInfo endPointInfo)
		{
			var uri = ConstructEndpoint(endPointInfo);

			switch (endPointInfo.HttpMethod.ToUpperInvariant())
			{
				case "GET":
					var getRequest = GetRequest(endPointInfo, uri);
					return HttpClient.Get(getRequest).Body;
				case "POST":
					var postRequest = PostRequest(endPointInfo, uri);
					return HttpClient.Post(postRequest).Body;
				default:
					throw new NotImplementedException();
			}
		}

		private Request GetRequest(EndPointInfo endPointInfo, string uri)
		{
			var signedUrl = SignHttpGetUrl(uri, endPointInfo);
			var getRequest = new Request(signedUrl, endPointInfo.Headers);
			return getRequest;
		}

		private Request PostRequest(EndPointInfo endPointInfo, string uri)
		{
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

		private string SignHttpGetUrl(string uri, EndPointInfo endPointInfo)
		{
			if (endPointInfo.IsSigned)
			{
				return _urlSigner.SignGetUrl(uri, endPointInfo.UserToken, endPointInfo.UserSecret, _oAuthCredentials);
			}

			return uri;
		}


		public virtual void HitEndpointAsync(EndPointInfo endPointInfo, Action<string> payload)
		{
			var uri = ConstructEndpoint(endPointInfo);

			switch (endPointInfo.HttpMethod.ToUpperInvariant())
			{
				case "GET":
					var getRequest = GetRequest(endPointInfo, uri);
					HttpClient.GetAsync(getRequest, (response) => payload(response.Body));
					break;
				case "POST":
					var postRequest = PostRequest(endPointInfo, uri);
					HttpClient.PostAsync(postRequest, (response) => payload(response.Body));
					break;
				default:
					throw new NotImplementedException();
			}
		}
	}
}