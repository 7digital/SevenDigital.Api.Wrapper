using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using NUnit.Framework;
using SevenDigital.Api.Wrapper.Requests;
using SevenDigital.Api.Wrapper.Responses;

namespace SevenDigital.Api.Wrapper.Unit.Tests.Responses.Parsing
{
	[TestFixture]
	public class CacheHeaderReaderTests
	{
		private readonly CacheHeaderReader _cacheHeaderReader = new CacheHeaderReader();

		[TestCase("max-age=60", 60)]
		[TestCase("max-age= 65 ", 65)]
		[TestCase("max-age=120", 120)]
		[TestCase("max-age=45 private", 45)]
		[TestCase("max-age=50   ", 50)]
		public void CanReadCacheDurationfromValidHeader(string headerText, int expectedValue)
		{
			var request = MakeRequest(HttpMethod.Get);

			var responseHeaders = CacheControlHeader(headerText);
			var response = new Response(HttpStatusCode.OK, responseHeaders, string.Empty, request);

			var expiration = _cacheHeaderReader.GetExpiration(response);
			Assert.That(expiration.HasValue, Is.True);

			var duration = expiration.Value.DateTime - DateTime.UtcNow;
			var seconds = duration.TotalSeconds;
			var maxAge = (int)Math.Round(seconds);
			Assert.That(maxAge, Is.EqualTo(expectedValue));
		}

		[TestCase("max-age=0")]
		[TestCase("max-age = 0")]
		[TestCase("no-store")]
		[TestCase("no-cache")]
		[TestCase("max-age=60 no-store")]
		[TestCase("max-age=60 no-cache")]
		public void CanReadNoCacheFromValidHeaders(string headerText)
		{
			var request = MakeRequest(HttpMethod.Get);

			var responseHeaders = CacheControlHeader(headerText);
			var response = new Response(HttpStatusCode.OK, responseHeaders, string.Empty, request);

			AssertNotCached(response);
		}

		[TestCase((string)null)]
		[TestCase("")]
		[TestCase("   ")]
		[TestCase("foo bar")]
		[TestCase("max-age=foo")]
		public void CanReadNoCacheFromInvalidHeaders(string headerText)
		{
			var request = MakeRequest(HttpMethod.Get);

			var responseHeaders = CacheControlHeader(headerText);
			var response = new Response(HttpStatusCode.OK, responseHeaders, string.Empty, request);

			AssertNotCached(response);
		}

		[Test]
		public void NotCacheableWhenRequestIsAPost()
		{
			var request = MakeRequest(HttpMethod.Post);

			var responseHeaders = CacheControlHeader("max-age=60");
			var response = new Response(HttpStatusCode.OK, responseHeaders, string.Empty, request);

			AssertNotCached(response);
		}

		[Test]
		public void NotCacheableWhenRequestIsAPut()
		{
			var request = MakeRequest(HttpMethod.Put);

			var responseHeaders = CacheControlHeader("max-age=60");
			var response = new Response(HttpStatusCode.OK, responseHeaders, string.Empty, request);

			AssertNotCached(response);
		}

		[Test]
		public void NotCacheableWhenThereIsNoCacheControlHeader()
		{
			var request = MakeRequest(HttpMethod.Get);

			var responseHeaders = new Dictionary<string, string>();
			var response = new Response(HttpStatusCode.OK, responseHeaders, string.Empty, request);

			AssertNotCached(response);
		}

		private static Dictionary<string, string> CacheControlHeader(string cacheControlValue)
		{
			return new Dictionary<string, string>
			{
				{"cache-control", cacheControlValue }
			};
		}

		private static Request MakeRequest(HttpMethod method)
		{
			var requestHeaders = new Dictionary<string, string>();
			var request = new Request(method, "http://some.url.com/foo/bar", requestHeaders,
				new RequestPayload(string.Empty, string.Empty));
			return request;
		}

		private void AssertNotCached(Response response)
		{
			var expiration = _cacheHeaderReader.GetExpiration(response);
			Assert.That(expiration.HasValue, Is.False, "expiration should not have a value");
		}
	}
}
