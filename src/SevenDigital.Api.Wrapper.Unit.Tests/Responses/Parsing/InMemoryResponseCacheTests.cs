using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Runtime.Caching;
using NUnit.Framework;
using SevenDigital.Api.Wrapper.Requests;
using SevenDigital.Api.Wrapper.Responses;

namespace SevenDigital.Api.Wrapper.Unit.Tests.Responses.Parsing
{
	[TestFixture]
	public class InMemoryResponseCacheTests
	{
		private InMemoryResponseCache _responseCache;

		[SetUp]
		public void Setup()
		{
			var tempMemoryCache = new MemoryCache(Guid.NewGuid().ToString());
			_responseCache = new InMemoryResponseCache(tempMemoryCache);
		}

		[Test]
		public void ValueWithAMaxAgeShouldBePresentInCache()
		{
			var storedValue = new object();

			var request = MakeRequest("value1");
			var response = CachableResponse(request);

			_responseCache.Set(response, storedValue);

			object retrievedValue;
			var retrieved = _responseCache.TryGet(request, out retrievedValue);

			Assert.That(retrieved, Is.True);
			Assert.That(retrievedValue, Is.Not.Null);
			Assert.That(retrievedValue, Is.EqualTo(storedValue));
		}

		[Test]
		public void ValueInCacheShouldNotbeRetrievedWithDifferentUrl()
		{
			var storedValue = new object();

			var request1 = MakeRequest("value1");
			var request2 = MakeRequest("value2");
			var response = CachableResponse(request1);

			_responseCache.Set(response, storedValue);

			object retrievedValue;
			var retrieved = _responseCache.TryGet(request2, out retrievedValue);

			Assert.That(retrieved, Is.False);
			Assert.That(retrievedValue, Is.Null);
		}

		[Test]
		public void ValueWithAMaxAgeOfZeroShouldNotBePresentInCache()
		{
			var storedValue = new object();

			var request = MakeRequest("value1");
			var responseHeaders = CacheControlHeader("max-age=0");
			var response = new Response(HttpStatusCode.OK, responseHeaders, string.Empty, request);

			_responseCache.Set(response, storedValue);

			object retrievedValue;
			var retrieved = _responseCache.TryGet(request, out retrievedValue);

			Assert.That(retrieved, Is.False);
			Assert.That(retrievedValue, Is.Null);
		}

		private static Request MakeRequest(string suffix)
		{
			var requestHeaders = new Dictionary<string, string>();
			var request = new Request(HttpMethod.Get, "http://some.url.com/foo/" + suffix, requestHeaders,
				new RequestPayload(string.Empty, string.Empty), null);
			return request;
		}

		private static Dictionary<string, string> CacheControlHeader(string cacheControlValue)
		{
			return new Dictionary<string, string>
			{
				{"cache-control", cacheControlValue }
			};
		}

		private static Response CachableResponse(Request request)
		{
			var responseHeaders = CacheControlHeader("max-age=60");
			var response = new Response(HttpStatusCode.OK, responseHeaders, string.Empty, request);
			return response;
		}
	}
}
