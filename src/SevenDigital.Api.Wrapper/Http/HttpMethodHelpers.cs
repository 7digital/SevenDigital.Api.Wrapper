using System;
using System.Net.Http;

namespace SevenDigital.Api.Wrapper.Http
{
	public static class HttpMethodHelpers
	{
		public static HttpMethod Parse(string methodName)
		{
			switch (methodName.ToUpperInvariant())
			{
				case "GET":
					return HttpMethod.Get;
				case "POST":
					return HttpMethod.Post;
				case "PUT":
					return HttpMethod.Put;
				case "DELETE":
					return HttpMethod.Delete;
				default:
					throw new ArgumentException("cannot parse '" + methodName + "' as a http method");
			}
		}

		public static bool ShouldHaveRequestBody(this HttpMethod method)
		{
			return method == HttpMethod.Post || method == HttpMethod.Put;
		}
	}
}
