using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net;
using FakeItEasy;
using NUnit.Framework;
using SevenDigital.Api.Schema;
using SevenDigital.Api.Wrapper.EndpointResolution.RequestHandlers;
using SevenDigital.Api.Wrapper.Exceptions;
using SevenDigital.Api.Wrapper.Http;
using SevenDigital.Api.Wrapper.Unit.Tests.Http;

namespace SevenDigital.Api.Wrapper.Unit.Tests
{
	[TestFixture]
	public class FluentApiTests
	{
		private const string VALID_STATUS_XML = "<?xml version=\"1.0\" encoding=\"utf-8\" ?><response status=\"ok\" version=\"1.2\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:noNamespaceSchemaLocation=\"http://api.7digital.com/1.2/static/7digitalAPI.xsd\"><serviceStatus><serverTime>2011-05-31T15:31:22Z</serverTime></serviceStatus></response>";

		private readonly Response stubResponse = new Response(HttpStatusCode.OK, VALID_STATUS_XML);

		[Test]
		public void Should_fire_requesthandler_with_correct_endpoint_on_resolve()
		{
			var requestHandler = A.Fake<IRequestHandler>();
			A.CallTo(() => requestHandler.HitEndpoint(A<RequestData>.Ignored)).Returns(stubResponse);

			new FluentApi<Status>(requestHandler).Please();

			Expression<Func<Response>> callWithEndpointStatus =
				() => requestHandler.HitEndpoint(A<RequestData>.That.Matches(x => x.Endpoint == "status"));

			A.CallTo(callWithEndpointStatus).MustHaveHappened(Repeated.Exactly.Once);
		}

		[Test]
		public void Should_fire_requesthandler_with_correct_methodname_on_resolve()
		{
			var requestHandler = A.Fake<IRequestHandler>();
			A.CallTo(() => requestHandler.HitEndpoint(A<RequestData>.Ignored)).Returns(stubResponse);

			new FluentApi<Status>(requestHandler).WithMethod("POST").Please();

			Expression<Func<Response>> callWithMethodPost =
				() => requestHandler.HitEndpoint(A<RequestData>.That.Matches(x => x.HttpMethod == HttpMethod.Post));

			A.CallTo(callWithMethodPost).MustHaveHappened(Repeated.Exactly.Once);
		}

		[Test]
		public void Should_recongise_standard_http_methods()
		{
			var requestHandler = A.Fake<IRequestHandler>();
			var api = new FluentApi<Status>(requestHandler);

			api.WithMethod("GET");
			api.WithMethod("POST");
			api.WithMethod("PUT");
			api.WithMethod("DELETE");
		}

		[Test]
		public void Should_fail_when_http_method_is_unrecognised()
		{
			var requestHandler = A.Fake<IRequestHandler>();
			var api = new FluentApi<Status>(requestHandler);
			Assert.Throws<ArgumentException>(() => api.WithMethod("FOO"));
		}

		[Test]
		public void Should_fire_requesthandler_with_correct_parameters_on_resolve()
		{
			var requestHandler = A.Fake<IRequestHandler>();
			A.CallTo(() => requestHandler.HitEndpoint(A<RequestData>.Ignored)).Returns(stubResponse);

			new FluentApi<Status>(requestHandler).WithParameter("artistId", "123").Please();

			Expression<Func<Response>> callWithArtistId123 =
				() => requestHandler.HitEndpoint(A<RequestData>.That.Matches(x => x.Parameters["artistId"] == "123"));

			A.CallTo(callWithArtistId123).MustHaveHappened();
		}

		[Test]
		public void Should_use_custom_http_client()
		{
			var requestHandler = A.Fake<IRequestHandler>();
			var fakeHttpClient = new FakeHttpClient();

			new FluentApi<Status>(requestHandler).UsingClient(fakeHttpClient);

			Assert.That(requestHandler.HttpClient, Is.EqualTo(fakeHttpClient));
		}

		[Test]
		public void Should_throw_exception_when_null_client_is_used()
		{
			var requestHandler = A.Fake<IRequestHandler>();
			var api = new FluentApi<Status>(requestHandler);

			Assert.Throws<ArgumentNullException>(() => api.UsingClient(null));
		}

		[Test]
		public void Should_wrap_webexception_under_api_exception_to_be_able_to_know_the_URL()
		{
			const string url = "http://foo.bar.baz/status";

			var requestHandler = A.Fake<IRequestHandler>();
			A.CallTo(() => requestHandler.HitEndpoint(A<RequestData>.Ignored)).Throws<WebException>();
			A.CallTo(() => requestHandler.GetDebugUri(A<RequestData>.Ignored)).Returns(url);

			var ex = Assert.Throws<ApiWebException>(() => new FluentApi<Status>(requestHandler).Please());

			Assert.That(ex.InnerException, Is.Not.Null);
			Assert.That(ex.Uri, Is.EqualTo(url));
			Assert.That(ex.InnerException.GetType(), Is.EqualTo(typeof(WebException)));
		}

		[Test]
		public void Should_throw_exception_when_null_cache_is_used()
		{
			var requestHandler = A.Fake<IRequestHandler>();
			var api = new FluentApi<Status>(requestHandler);

			Assert.Throws<ArgumentNullException>(() => api.UsingCache(null));
		}

		[Test]
		public void Should_read_cache()
		{
			var requestHandler = A.Fake<IRequestHandler>();
			var cache = new FakeCache();
			A.CallTo(() => requestHandler.HitEndpoint(A<RequestData>.Ignored))
				.Returns(stubResponse);

			new FluentApi<Status>(requestHandler).UsingCache(cache).Please();

			Assert.That(cache.TryGetCount, Is.EqualTo(1));
		}

		[Test]
		public void Should_write_to_cache_on_success()
		{
			var requestHandler = A.Fake<IRequestHandler>();
			var cache = new FakeCache();
			A.CallTo(() => requestHandler.HitEndpoint(A<RequestData>.Ignored))
				.Returns(stubResponse);

			new FluentApi<Status>(requestHandler).UsingCache(cache).Please();

			Assert.That(cache.SetCount, Is.EqualTo(1));
			Assert.That(cache.CachedResponses.Count, Is.EqualTo(1));
			Assert.That(cache.CachedResponses[0], Is.EqualTo(stubResponse));
		}

		[Test]
		public void Should_return_value_from_cache()
		{
			var requestHandler = A.Fake<IRequestHandler>();
			var cache = new FakeCache();
			cache.StubResponse = stubResponse;

			var status =new FluentApi<Status>(requestHandler).UsingCache(cache).Please();

			Assert.That(cache.TryGetCount, Is.EqualTo(1));
			Assert.That(status, Is.Not.Null);
		}

		[Test]
		public void Should_not_hit_endpoint_when_value_is_found_in_cache()
		{
			var requestHandler = A.Fake<IRequestHandler>();
			var cache = new FakeCache();
			cache.StubResponse = stubResponse;

			new FluentApi<Status>(requestHandler).UsingCache(cache).Please();
			A.CallTo(() => requestHandler.HitEndpoint(A<RequestData>.Ignored)).MustNotHaveHappened();
		}

		[Test]
		public void Should_not_write_to_cache_on_failure()
		{
			var requestHandler = A.Fake<IRequestHandler>();
			var cache = new FakeCache();
			A.CallTo(() => requestHandler.HitEndpoint(A<RequestData>.Ignored)).Throws<WebException>();
			A.CallTo(() => requestHandler.GetDebugUri(A<RequestData>.Ignored)).Returns("http://foo.com/bar");

			var api = new FluentApi<Status>(requestHandler).UsingCache(cache);

			Assert.Throws<ApiWebException>(() => api.Please());

			Assert.That(cache.SetCount, Is.EqualTo(0));
			Assert.That(cache.CachedResponses.Count, Is.EqualTo(0));
		}
	}

	internal class FakeCache: IResponseCache
	{
		public int SetCount { get; set; }
		public int TryGetCount { get; set; }
		public IList<Response> CachedResponses { get; set; }

		public Response StubResponse { get; set; }

		internal FakeCache()
		{
			CachedResponses = new List<Response>();
		}

		public void Set(RequestData key, Response value)
		{
			SetCount++;
			CachedResponses.Add(value);
		}

		public bool TryGet(RequestData key, out Response value)
		{
			TryGetCount++;
			value = StubResponse;
			return (StubResponse != null);
		}
	}
}
