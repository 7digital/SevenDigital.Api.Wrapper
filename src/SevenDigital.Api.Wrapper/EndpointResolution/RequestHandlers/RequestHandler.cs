using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using SevenDigital.Api.Wrapper.Http;

namespace SevenDigital.Api.Wrapper.EndpointResolution.RequestHandlers
{
	public abstract class RequestHandler
	{
		public abstract Response HitEndpoint(RequestData requestData);
		public abstract void HitEndpointAsync(RequestData requestData, Action<Response> action);
		public abstract string GetDebugUri(RequestData requestData);

		private readonly IApiUri _apiUri;

		protected RequestHandler(IApiUri apiUri)
		{
			_apiUri = apiUri;
		}

		public abstract bool HandlesMethod(string method);

		public IHttpClient HttpClient { get; set; }

		protected ApiRequest MakeApiRequest(RequestData requestData)
		{
			var apiBaseUri = requestData.UseHttps ? _apiUri.SecureUri : _apiUri.Uri;

			var parameters = CopyParameters(requestData);

			var uriString = string.Format("{0}/{1}", apiBaseUri, SubstituteRouteParametersAndRemoveFromQuery(requestData.Endpoint, parameters));

			return new ApiRequest {AbsoluteUrl = uriString, Parameters = parameters};
		}

		private static Dictionary<string, string> CopyParameters(RequestData requestData)
		{
			var newDictionary = requestData.Parameters.ToDictionary(entry => entry.Key, entry => entry.Value);
			return newDictionary;
		}

		private static string SubstituteRouteParametersAndRemoveFromQuery(string endpointUri, Dictionary<string, string> parameters)
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

	public class ApiRequest
	{
		public string AbsoluteUrl { get; set; }
		public IDictionary<string, string> Parameters { get; set; }

		public string FullUrl
		{
			get { return AbsoluteUrl + "?" + Parameters.ToQueryString(true); }
		}
	}
}
