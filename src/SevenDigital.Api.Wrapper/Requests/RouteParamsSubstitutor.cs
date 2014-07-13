using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace SevenDigital.Api.Wrapper.Requests
{
	public class RouteParamsSubstitutor
	{
		private readonly IApiUri _apiUri;

		public RouteParamsSubstitutor(IApiUri apiUri)
		{
			_apiUri = apiUri;
		}

		public ApiRequest SubstituteParamsInRequest(RequestData requestData)
		{
			var apiBaseUrl = GetApiBaseUrl(requestData);

			var withoutRouteParameters = new Dictionary<string, string>(requestData.Parameters);

			var pathWithRouteParamsSubstituted = SubstituteRouteParameters(requestData.Endpoint, withoutRouteParameters);

			return new ApiRequest
			{
				AbsoluteUrl = string.Format("{0}/{1}", apiBaseUrl, pathWithRouteParamsSubstituted),
				Parameters = withoutRouteParameters
			};
		}

		private string GetApiBaseUrl(RequestData requestData)
		{
			if (requestData.BaseUriProvider != null)
			{
				var providedUrl = requestData.BaseUriProvider.BaseUri();
				if (! string.IsNullOrEmpty(providedUrl))
				{
					return providedUrl;
				}
			}

			if (requestData.UseHttps)
			{
				return _apiUri.SecureUri;
			}

			return _apiUri.Uri;
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
