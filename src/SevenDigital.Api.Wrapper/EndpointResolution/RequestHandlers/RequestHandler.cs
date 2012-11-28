using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using SevenDigital.Api.Wrapper.Http;

namespace SevenDigital.Api.Wrapper.EndpointResolution.RequestHandlers
{
	public abstract class RequestHandler
	{
		public abstract Response HitEndpoint(EndpointContext endpointContext, RequestContext requestContext);
		public abstract void HitEndpointAsync(EndpointContext endpointContext, RequestContext requestContext, Action<Response> action);
		protected abstract string AdditionalParameters(Dictionary<string, string> newDictionary);

		private readonly IApiUri _apiUri;

		protected RequestHandler(IApiUri apiUri)
		{
			_apiUri = apiUri;
		}

		public IHttpClient HttpClient { get; set; }

		public virtual string ConstructEndpoint(EndpointContext endpointContext, RequestContext requestContext)
		{
			var apiUri = endpointContext.UseHttps ? _apiUri.SecureUri : _apiUri.Uri;

			var newDictionary = requestContext.Parameters.ToDictionary(entry => entry.Key, entry => entry.Value);

			var uriString = string.Format("{0}/{1}", apiUri, SubstituteRouteParameters(endpointContext.UriPath, newDictionary));

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
	}
}
