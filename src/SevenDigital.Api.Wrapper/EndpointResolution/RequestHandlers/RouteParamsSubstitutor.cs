using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using SevenDigital.Api.Wrapper.Http;

namespace SevenDigital.Api.Wrapper.EndpointResolution.RequestHandlers
{
	public class RouteParamsSubstitutor
	{
		public static ApiRequest SubstituteParamsInRequest(IApiUri apiUri, RequestData requestData)
		{
			var apiBaseUrl = requestData.UseHttps ? apiUri.SecureUri : apiUri.Uri;

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
