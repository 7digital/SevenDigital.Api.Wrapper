using System;
using System.Collections.Generic;
using System.Linq;
using SevenDigital.Api.Schema.Attributes;
using SevenDigital.Api.Schema.OAuth;
using SevenDigital.Api.Wrapper.Http;

namespace SevenDigital.Api.Wrapper.AttributeManagement
{
	public class AttributeRequestDataBuilder
	{
		public static RequestData BuildRequestData<T>()
		{
			var requestData = new RequestData();

			var customAttributes = typeof(T).GetCustomAttributes(true);

			requestData.UriPath = ParseApiEndpointAttribute<T>(customAttributes);
			requestData.IsSigned = ParseOAuthSignedAttribute(customAttributes);
			requestData.UseHttps = ParseRequireSecureAttribute(customAttributes);

			if (ParseHttpPostAttribute(customAttributes) != null)
			{
				requestData.HttpMethod = "POST";
			}

			return requestData;
		}

		private static string ParseApiEndpointAttribute<T>(IEnumerable<object> customAttributes)
		{
			var attribute = customAttributes
				.OfType<ApiEndpointAttribute>()
				.FirstOrDefault();

			if (attribute == null)
			{
				throw new ArgumentException(string.Format("Theis Type {0} cannot be used in this way, it has no ApiEndpointAttribute", typeof (T)));
			}

			return attribute.EndpointUri;
		}

		private static bool ParseOAuthSignedAttribute(IEnumerable<object> customAttributes)
		{
			var isSigned = customAttributes
				.OfType<OAuthSignedAttribute>()
				.FirstOrDefault();

			return isSigned != null;
		}

		private static bool ParseRequireSecureAttribute(IEnumerable<object> customAttributes)
		{
			var isSecure = customAttributes
				.OfType<RequireSecureAttribute>()
				.FirstOrDefault();

			return isSecure != null;
		}

		private static string ParseHttpPostAttribute(IEnumerable<object> customAttributes)
		{
			var isHttpPost = customAttributes
				.OfType<HttpPostAttribute>()
				.FirstOrDefault();

			return isHttpPost != null ? "POST" : null;
		}
	}
}