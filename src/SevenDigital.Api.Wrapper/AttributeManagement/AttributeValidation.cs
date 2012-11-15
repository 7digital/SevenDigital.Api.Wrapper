using System;
using System.Linq;
using SevenDigital.Api.Schema.Attributes;
using SevenDigital.Api.Schema.OAuth;
using SevenDigital.Api.Wrapper.Http;

namespace SevenDigital.Api.Wrapper.AttributeManagement
{
	public class AttributeValidation<T>
	{
		public RequestData Validate()
		{
			var requestData = new RequestData();

			requestData.UriPath = ParseApiEndpointAttribute();
			requestData.IsSigned = ParseOAuthSignedAttribute();
			requestData.UseHttps = ParseRequireSecureAttribute();

			if (ParseHttpPostAttribute() != null)
			{
				requestData.HttpMethod = "POST";
			}

			return requestData;
		}

		private static string ParseApiEndpointAttribute()
		{
			var attribute = typeof (T).GetCustomAttributes(true)
				.OfType<ApiEndpointAttribute>()
				.FirstOrDefault();

			if (attribute == null)
			{
				throw new ArgumentException(string.Format("The Type {0} cannot be used in this way, it has no ApiEndpointAttribute", typeof (T)));
			}

			return attribute.EndpointUri;
		}

		private static bool ParseOAuthSignedAttribute()
		{
			var isSigned = typeof(T).GetCustomAttributes(true)
				.OfType<OAuthSignedAttribute>()
				.FirstOrDefault();

			return isSigned != null;
		}

		private static bool ParseRequireSecureAttribute()
		{
			var isSecure = typeof(T).GetCustomAttributes(true)
				.OfType<RequireSecureAttribute>()
				.FirstOrDefault();

			return isSecure != null;
		}

		private static string ParseHttpPostAttribute()
		{
			var isHttpPost = typeof(T).GetCustomAttributes(true)
				.OfType<HttpPostAttribute>()
				.FirstOrDefault();

			return isHttpPost != null ? "POST" : null;
		}
	}
}