using System.Collections.Generic;
using System.Net;
using SevenDigital.Api.Wrapper.Responses;

namespace SevenDigital.Api.Wrapper.Unit.Tests
{
	public static class ResponseCreator
	{
		public static Response FromBody(HttpStatusCode statusCode, string body)
		{
			var headers = new Dictionary<string, string>(){{"Content-Type", "application/xml"}};
			return new Response(statusCode, headers,body);
		}
	}
}