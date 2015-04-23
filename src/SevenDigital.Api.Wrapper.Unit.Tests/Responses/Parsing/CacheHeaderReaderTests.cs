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

		[TestCase("max-age: 60", 60)]
		[TestCase("max-age: 65 ", 65)]
		[TestCase("max-age:120", 120)]
		[TestCase("max-age:0", 0)]
		[TestCase("max-age: 0", 0)]
		[TestCase("max-age: 45 private", 45)]
		public void CanReadMaxAgefromHeaders(string headerText, int expectedValue)
		{
			var request = MakeRequest(HttpMethod.Get);

			var responseHeaders = CacheControlHeader(headerText);
			var response = new Response(HttpStatusCode.OK, responseHeaders, string.Empty, request);

			var expiration = _cacheHeaderReader.GetExpiration(response);
			Assert.That(expiration.HasValue, Is.EqualTo(expectedValue > 0));

			if (expiration.HasValue)
			{
				var duration = expiration.Value.DateTime - DateTime.UtcNow;
				var seconds = duration.TotalSeconds;
				var maxAge = (int)Math.Round(seconds);
				Assert.That(maxAge, Is.EqualTo(expectedValue));
			}
		}

		[Test]
		public void NotCacheableWhenRequestIsAPost()
		{
			var request = MakeRequest(HttpMethod.Post);

			var responseHeaders = CacheControlHeader("max-age: 60 private");
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

		[Test]
		public void NotCacheableWhenCacheControlHeaderIsEmpty()
		{
			var request = MakeRequest(HttpMethod.Get);

			var responseHeaders = CacheControlHeader(string.Empty);
			var response = new Response(HttpStatusCode.OK, responseHeaders, string.Empty, request);

			AssertNotCached(response);
		}

		[Test]
		public void NotCacheableWhenCacheControlHeaderIsUnknownText()
		{
			var request = MakeRequest(HttpMethod.Get);

			var responseHeaders = CacheControlHeader("foo bar fish");
			var response = new Response(HttpStatusCode.OK, responseHeaders, string.Empty, request);

			AssertNotCached(response);
		}

		[Test]
		public void NotCacheableWhenCacheControlHeaderMaxAgeIsNotANumber()
		{
			var request = MakeRequest(HttpMethod.Get);

			var responseHeaders = CacheControlHeader("max-age:foo");
			var response = new Response(HttpStatusCode.OK, responseHeaders, string.Empty, request);

			AssertNotCached(response);
		}

		[Test]
		public void NotCacheableWhenNoCacheIsSpecified()
		{
			var request = MakeRequest(HttpMethod.Get);

			var responseHeaders = CacheControlHeader("no-cache max-age: 30");
			var response = new Response(HttpStatusCode.OK, responseHeaders, string.Empty, request);

			AssertNotCached(response);
		}

		[Test]
		public void NotCacheableWhenNoStoreIsSpecified()
		{
			var request = MakeRequest(HttpMethod.Get);

			var responseHeaders = CacheControlHeader("no-store max-age: 30");
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
