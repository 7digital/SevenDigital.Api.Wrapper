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
		public abstract bool HandlesMethod(HttpMethod method);

		private readonly IApiUri _apiUri;

		protected RequestHandler(IApiUri apiUri)
		{
			_apiUri = apiUri;
		}
		
		public IHttpClient HttpClient { get; set; }

		protected ApiRequest MakeApiRequest(RequestData requestData)
		{
			var apiBaseUrl = requestData.UseHttps ? _apiUri.SecureUri : _apiUri.Uri;

			var withoutRouteParameters = new Dictionary<string, string>(requestData.Parameters);

			var pathWithRouteParamsSubstituted = SubstituteRouteParameters(requestData.Endpoint, withoutRouteParameters);

			return new ApiRequest
				{
					AbsoluteUrl = string.Format("{0}/{1}", apiBaseUrl, pathWithRouteParamsSubstituted),
					Parameters = withoutRouteParameters
				};
		}
		
		private static string SubstituteRouteParameters(string endpointUri, IDictionary<string, string> parameters)
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
