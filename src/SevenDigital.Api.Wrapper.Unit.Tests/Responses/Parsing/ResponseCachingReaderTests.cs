using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using NUnit.Framework;
using SevenDigital.Api.Wrapper.Requests;
using SevenDigital.Api.Wrapper.Responses;

namespace SevenDigital.Api.Wrapper.Unit.Tests.Responses.Parsing
{
	[TestFixture]
	public class ResponseCachingReaderTests
	{
		[Test]
		public void CanReadMaxAge()
		{
			var requestHeaders = new Dictionary<string, string>();
			
			var responseHeaders = new Dictionary<string, string>
				{
					{"cache-control", "max-age: 60 private"}
				}; 
			
			var request = new Request(HttpMethod.Get, "http://some.url", requestHeaders,
				new RequestPayload(string.Empty, string.Empty));

			var response = new Response(HttpStatusCode.OK, responseHeaders, string.Empty, request);

			var maxAge = ResponseCachingReader.DurationSeconds(response);

			Assert.That(maxAge, Is.EqualTo(60));
		}
	}
}
