using System;
using System.Linq;
using System.Net.Http;
using SevenDigital.Api.Schema.Attributes;
using SevenDigital.Api.Schema.OAuth;
using SevenDigital.Api.Wrapper.Requests;

namespace SevenDigital.Api.Wrapper.Environment
{
	public class AttributeRequestDataBuilder<T>
	{
		public RequestData BuildRequestData()
		{
			var requestData = new RequestData();

			requestData.Endpoint = ParseApiEndpointAttribute();
			requestData.RequiresSignature = ParseOAuthSignedAttribute();
			requestData.UseHttps = ParseRequireSecureAttribute();
			requestData.HttpMethod = HttpMethodfromAttributes();

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

		private static HttpMethod HttpMethodfromAttributes()
		{
			var attrs = typeof(T).GetCustomAttributes(true);

			if (attrs.OfType<HttpPostAttribute>().Any())
			{
				return HttpMethod.Post;
			}

			if (attrs.OfType<HttpDeleteAttribute>().Any())
			{
				return HttpMethod.Delete;
			}

			if (attrs.OfType<HttpPutAttribute>().Any())
			{
				return HttpMethod.Put;
			}

			return HttpMethod.Get;
		}
	}
}