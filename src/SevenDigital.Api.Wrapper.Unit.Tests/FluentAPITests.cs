using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net;
using FakeItEasy;
using NUnit.Framework;
using SevenDigital.Api.Schema;
using SevenDigital.Api.Wrapper.Exceptions;
using SevenDigital.Api.Wrapper.Http;
using SevenDigital.Api.Wrapper.Requests;
using SevenDigital.Api.Wrapper.Responses;
using SevenDigital.Api.Wrapper.Unit.Tests.Http;

namespace SevenDigital.Api.Wrapper.Unit.Tests
{
	[TestFixture]
	public class FluentApiTests
	{
		private const string VALID_STATUS_XML = "<?xml version=\"1.0\" encoding=\"utf-8\" ?><response status=\"ok\" version=\"1.2\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:noNamespaceSchemaLocation=\"http://api.7digital.com/1.2/static/7digitalAPI.xsd\"><serviceStatus><serverTime>2011-05-31T15:31:22Z</serverTime></serviceStatus></response>";

		private readonly Response stubResponse = new Response(HttpStatusCode.OK, VALID_STATUS_XML);

		private IRequestBuilder StubRequestBuilder()
		{
			var request = new Request(HttpMethod.Get, "http://example.com/status", new Dictionary<string, string>(), string.Empty);
			var requestBuilder = A.Fake<IRequestBuilder>();
			A.CallTo(() => requestBuilder.BuildRequest(A<RequestData>.Ignored)).Returns(request);

			return requestBuilder;
		}

		private IHttpClient StubHttpClient()
		{
			var httpClient = A.Fake<IHttpClient>();
			A.CallTo(() => httpClient.Send(A<Request>.Ignored)).Returns(stubResponse);
			return httpClient;
		}

		[Test] public void Should_call_requestbuilder_with_correct_endpoint_on_resolve()
		{
			var requestBuilder = StubRequestBuilder();
			var httpClient = StubHttpClient();

			new FluentApi<Status>(httpClient, requestBuilder).Please();

			Expression<Func<Request>> callWithEndpointStatus =
				() => requestBuilder.BuildRequest(A<RequestData>.That.Matches(x => x.Endpoint == "status"));

			A.CallTo(callWithEndpointStatus).MustHaveHappened(Repeated.Exactly.Once);
		}

		[Test]
		public void Should_fire_requesthandler_with_correct_methodname_on_resolve()
		{
			var requestHandler = StubRequestBuilder();
			var httpClient = StubHttpClient();

			new FluentApi<Status>(httpClient, requestHandler).WithMethod("POST").Please();

			Expression<Func<Request>> callWithMethodPost =
				() => requestHandler.BuildRequest(A<RequestData>.That.Matches(x => x.HttpMethod == HttpMethod.Post));

			A.CallTo(callWithMethodPost).MustHaveHappened(Repeated.Exactly.Once);
		}

		[Test]
		public void Should_recongise_standard_http_methods()
		{
			var requestHandler = A.Fake<IRequestBuilder>();
			var api = new FluentApi<Status>(requestHandler);

			api.WithMethod("GET");
			api.WithMethod("POST");
			api.WithMethod("PUT");
			api.WithMethod("DELETE");
		}

		[Test]
		public void Should_fail_when_http_method_is_unrecognised()
		{
			var requestHandler = A.Fake<IRequestBuilder>();
			var api = new FluentApi<Status>(requestHandler);
			Assert.Throws<ArgumentException>(() => api.WithMethod("FOO"));
		}

		[Test]
		public void Should_fire_requesthandler_with_correct_parameters_on_resolve()
		{
			var requestBuilder = StubRequestBuilder();
			var httpClient = StubHttpClient();

			var api = new FluentApi<Status>(httpClient, requestBuilder);
			api.WithParameter("artistId", "123").Please();

			Expression<Func<Request>> callWithArtistId123 =
				() => requestBuilder.BuildRequest(A<RequestData>.That.Matches(x => x.Parameters["artistId"] == "123"));

			A.CallTo(callWithArtistId123).MustHaveHappened();
		}

		[Test]
		public void Should_use_custom_http_client()
		{
			var requestHandler = StubRequestBuilder();
			var fakeHttpClient = new FakeHttpClient(stubResponse);

			var api = new FluentApi<Status>(requestHandler).UsingClient(fakeHttpClient);
			Assert.That(fakeHttpClient.SendCount, Is.EqualTo(0));

			api.Please();

			Assert.That(fakeHttpClient.SendCount, Is.EqualTo(1));
		}

		[Test]
		public void Should_throw_exception_when_null_client_is_used()
		{
			var requestHandler = A.Fake<IRequestBuilder>();
			var api = new FluentApi<Status>(requestHandler);

			Assert.Throws<ArgumentNullException>(() => api.UsingClient(null));
		}

		[Test]
		public void Should_wrap_webexception_under_api_exception_to_be_able_to_know_the_URL()
		{
			const string url = "http://example.com/status";

			var requestHandler = StubRequestBuilder();
			var httpClient = StubHttpClient();
			A.CallTo(() => httpClient.Send(A<Request>.Ignored)).Throws<WebException>();

			var api = new FluentApi<Status>(httpClient, requestHandler);

			var ex = Assert.Throws<ApiWebException>(() => api.Please());

			Assert.That(ex.InnerException, Is.Not.Null);
			Assert.That(ex.Uri, Is.EqualTo(url));
			Assert.That(ex.InnerException.GetType(), Is.EqualTo(typeof(WebException)));
		}

		[Test]
		public void Should_throw_exception_when_null_cache_is_used()
		{
			var requestHandler = A.Fake<IRequestBuilder>();
			var httpClient = StubHttpClient();

			var api = new FluentApi<Status>(httpClient, requestHandler);

			Assert.Throws<ArgumentNullException>(() => api.UsingCache(null));
		}

		[Test]
		public void Should_read_cache()
		{
			var httpClient = StubHttpClient();
			var requestHandler = StubRequestBuilder();

			var api = new FluentApi<Status>(httpClient, requestHandler);

			var cache = new FakeCache();
			api.UsingCache(cache).Please();

			Assert.That(cache.TryGetCount, Is.EqualTo(1));
		}

		[Test]
		public void Should_write_to_cache_on_success()
		{
			var requestHandler = StubRequestBuilder();
			var httpClient = StubHttpClient();
			var api = new FluentApi<Status>(httpClient, requestHandler);
			
			var cache = new FakeCache();
			api.UsingCache(cache).Please();

			Assert.That(cache.SetCount, Is.EqualTo(1));
			Assert.That(cache.CachedResponses.Count, Is.EqualTo(1));
			Assert.That(cache.CachedResponses[0], Is.EqualTo(stubResponse));
		}

		[Test]
		public void Should_return_value_from_cache()
		{
			var requestHandler = StubRequestBuilder();
			var httpClient = StubHttpClient();
			var api = new FluentApi<Status>(httpClient, requestHandler);

			var cache = new FakeCache();
			var status = api.UsingCache(cache).Please();

			Assert.That(cache.TryGetCount, Is.EqualTo(1));
			Assert.That(status, Is.Not.Null);
		}

		[Test]
		public void Should_not_hit_endpoint_when_value_is_found_in_cache()
		{
			var requestHandler = StubRequestBuilder();
			var httpClient = StubHttpClient();
			var api = new FluentApi<Status>(httpClient, requestHandler);

			var cache = new FakeCache();
			cache.StubResponse = stubResponse;

			api.UsingCache(cache).Please();
			A.CallTo(() => httpClient.Send(A<Request>.Ignored)).MustNotHaveHappened();
		}

		[Test]
		public void Should_not_write_to_cache_on_failure()
		{
			var requestHandler = StubRequestBuilder();
			var httpClient = StubHttpClient();
			var api = new FluentApi<Status>(httpClient, requestHandler);

			var cache = new FakeCache();

			A.CallTo(() => httpClient.Send(A<Request>.Ignored)).Throws<WebException>();

			api.UsingCache(cache);

			Assert.Throws<ApiWebException>(() => api.Please());

			Assert.That(cache.SetCount, Is.EqualTo(0));
			Assert.That(cache.CachedResponses.Count, Is.EqualTo(0));
		}
	}
}
