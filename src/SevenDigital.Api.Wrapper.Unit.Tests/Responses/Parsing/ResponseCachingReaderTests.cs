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
		[TestCase("max-age: 60", 60)]
		[TestCase("max-age: 65 ", 65)]
		[TestCase("max-age:120", 120)]
		[TestCase("max-age:0", 0)]
		[TestCase("max-age: 0", 0)]
		[TestCase("max-age: 45 private", 45)]
		public void CanReadMaxAgefromHeaders(string headerText, int expectedValue)
		{
			var request = DummyRequest(HttpMethod.Get);

			var responseHeaders = CacheControlHeader(headerText);
			var response = new Response(HttpStatusCode.OK, responseHeaders, string.Empty, request);

			var isCachable = ResponseCachingReader.IsCachable(response);
			var maxAge = ResponseCachingReader.DurationSeconds(response);

			Assert.That(isCachable, Is.True);
			Assert.That(maxAge, Is.EqualTo(expectedValue));
		}

		[Test]
		public void DoesNotCachePost()
		{
			var request = DummyRequest(HttpMethod.Post);

			var responseHeaders = CacheControlHeader("max-age: 60 private");
			var response = new Response(HttpStatusCode.OK, responseHeaders, string.Empty, request);

			var isCachable = ResponseCachingReader.IsCachable(response);
			var maxAge = ResponseCachingReader.DurationSeconds(response);

			Assert.That(isCachable, Is.False);
			Assert.That(maxAge, Is.EqualTo(0));
		}

		[Test]
		public void ReturnsZeroWhenThereIsNoCacheControlHeader()
		{
			var request = DummyRequest(HttpMethod.Get);

			var responseHeaders = new Dictionary<string, string>();
			var response = new Response(HttpStatusCode.OK, responseHeaders, string.Empty, request);

			var isCachable = ResponseCachingReader.IsCachable(response);
			var maxAge = ResponseCachingReader.DurationSeconds(response);

			Assert.That(isCachable, Is.False);
			Assert.That(maxAge, Is.EqualTo(0));
		}

		[Test]
		public void ReturnsZeroWhenCacheControlHeaderIsEmpty()
		{
			var request = DummyRequest(HttpMethod.Get);

			var responseHeaders = CacheControlHeader(string.Empty);
			var response = new Response(HttpStatusCode.OK, responseHeaders, string.Empty, request);

			var maxAge = ResponseCachingReader.DurationSeconds(response);

			Assert.That(maxAge, Is.EqualTo(0));
		}

		[Test]
		public void ReturnsZeroWhenCacheControlHeaderIsInvalid()
		{
			var request = DummyRequest(HttpMethod.Get);

			var responseHeaders = CacheControlHeader("foo bar fish");
			var response = new Response(HttpStatusCode.OK, responseHeaders, string.Empty, request);

			var maxAge = ResponseCachingReader.DurationSeconds(response);

			Assert.That(maxAge, Is.EqualTo(0));
		}

		[Test]
		public void ReturnsZeroWhenCacheControlHeaderMaxAgeIsInvalid()
		{
			var request = DummyRequest(HttpMethod.Get);

			var responseHeaders = CacheControlHeader("max-age:foo");
			var response = new Response(HttpStatusCode.OK, responseHeaders, string.Empty, request);

			var maxAge = ResponseCachingReader.DurationSeconds(response);

			Assert.That(maxAge, Is.EqualTo(0));
		}

		[Test]
		public void ReturnsZeroWhenNoCacheIsSpecified()
		{
			var request = DummyRequest(HttpMethod.Get);

			var responseHeaders = CacheControlHeader("no-cache max-age: 30");
			var response = new Response(HttpStatusCode.OK, responseHeaders, string.Empty, request);

			var maxAge = ResponseCachingReader.DurationSeconds(response);

			Assert.That(maxAge, Is.EqualTo(0));
		}

		[Test]
		public void ReturnsZeroWhenNoStoreIsSpecified()
		{
			var request = DummyRequest(HttpMethod.Get);

			var responseHeaders = CacheControlHeader("no-store max-age: 30");
			var response = new Response(HttpStatusCode.OK, responseHeaders, string.Empty, request);

			var maxAge = ResponseCachingReader.DurationSeconds(response);

			Assert.That(maxAge, Is.EqualTo(0));
		}

		private static Dictionary<string, string> CacheControlHeader(string cacheControlValue)
		{
			return new Dictionary<string, string>
			{
				{"cache-control", cacheControlValue }
			};
		}

		private static Request DummyRequest(HttpMethod method)
		{
			var requestHeaders = new Dictionary<string, string>();
			var request = new Request(method, "http://some.url.com/foo/bar", requestHeaders,
				new RequestPayload(string.Empty, string.Empty));
			return request;
		}
	}
}
