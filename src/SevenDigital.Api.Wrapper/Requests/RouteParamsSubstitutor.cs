using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace SevenDigital.Api.Wrapper.Requests
{
	public class RouteParamsSubstitutor : IRouteParamsSubstitutor
	{
		private readonly IBaseUriProvider _defaultBaseUriProvider;

		public RouteParamsSubstitutor(IApiUri apiUri)
		{
			_defaultBaseUriProvider = new BaseUriFromApiUri(apiUri);
		}

		public ApiRequest SubstituteParamsInRequest(RequestData requestData)
		{
			var apiBaseUrl = GetApiBaseUrl(requestData);

			var withoutRouteParameters = new Dictionary<string, string>(requestData.Parameters);

			var pathWithRouteParamsSubstituted = SubstituteRouteParameters(requestData.Endpoint, withoutRouteParameters);

			return new ApiRequest
			{
				AbsoluteUrl = UriPath.Combine(apiBaseUrl, pathWithRouteParamsSubstituted),
				Parameters = withoutRouteParameters
			};
		}

		private string GetApiBaseUrl(RequestData requestData)
		{
			var baseUriProvider = requestData.BaseUriProvider ?? _defaultBaseUriProvider;

			return baseUriProvider.BaseUri(requestData).ToLower();
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

			return endpointUri;
		}
	}
}
