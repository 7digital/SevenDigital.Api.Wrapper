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
		protected abstract string AdditionalParameters(Dictionary<string, string> newDictionary);

		private readonly IApiUri _apiUri;

		protected RequestHandler(IApiUri apiUri)
		{
			_apiUri = apiUri;
		}

		public abstract bool HandlesMethod(string method);

		public IHttpClient HttpClient { get; set; }

		protected string ConstructEndpoint(RequestData requestData)
		{
			var apiUri = requestData.UseHttps ? _apiUri.SecureUri : _apiUri.Uri;

			var newDictionary = requestData.Parameters.ToDictionary(entry => entry.Key, entry => entry.Value);

			var uriString = string.Format("{0}/{1}", apiUri, SubstituteRouteParameters(requestData.Endpoint, newDictionary));

			uriString = uriString + AdditionalParameters(newDictionary);
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

		public string GetDebugUri(RequestData requestData)
		{
			return ConstructEndpoint(requestData);
		}
	}
}
