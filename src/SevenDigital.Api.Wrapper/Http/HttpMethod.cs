using System;
using System.Net.Http;

namespace SevenDigital.Api.Wrapper.Http
{
	public static class HttpMethodHelpers
	{
		public static HttpMethod Parse(string methodName)
		{
			return(HttpMethod)Enum.Parse(typeof (HttpMethod), methodName, true);
		}

		public static bool HasParamsInQueryString(this HttpMethod method)
		{
			return (method == HttpMethod.Get) || (method == HttpMethod.Delete);
		}

		public static bool ShouldHaveRequestBody(this HttpMethod method)
		{
			return (method == HttpMethod.Post) || (method == HttpMethod.Put);
		}
	}
}
