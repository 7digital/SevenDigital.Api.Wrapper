using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net;
using FakeItEasy;
using NUnit.Framework;
using SevenDigital.Api.Wrapper.EndpointResolution;
using SevenDigital.Api.Schema;
using System.Threading;
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
		public void Should_fire_requestcoordinator_with_correct_endpoint_on_resolve()
		{
			var requestCoordinator = A.Fake<IRequestCoordinator>();
			A.CallTo(() => requestCoordinator.HitEndpoint(A<RequestData>.Ignored)).Returns(stubResponse);

			new FluentApi<Status>(requestCoordinator).Please();

			Expression<Func<Response>> callWithEndpointStatus =
				() => requestCoordinator.HitEndpoint(A<RequestData>.That.Matches(x => x.Endpoint == "status"));

			A.CallTo(callWithEndpointStatus).MustHaveHappened(Repeated.Exactly.Once);
		}

		[Test]
		public void Should_fire_requestcoordinator_with_correct_methodname_on_resolve()
		{
			var requestCoordinator = A.Fake<IRequestCoordinator>();
			A.CallTo(() => requestCoordinator.HitEndpoint(A<RequestData>.Ignored)).Returns(stubResponse);

			new FluentApi<Status>(requestCoordinator).WithMethod("POST").Please();

			Expression<Func<Response>> callWithMethodPost =
				() => requestCoordinator.HitEndpoint(A<RequestData>.That.Matches(x => x.HttpMethod == "POST"));

			A.CallTo(callWithMethodPost).MustHaveHappened(Repeated.Exactly.Once);
		}

		[Test]
		public void Should_fire_requestcoordinator_with_correct_parameters_on_resolve()
		{
			var requestCoordinator = A.Fake<IRequestCoordinator>();
			A.CallTo(() => requestCoordinator.HitEndpoint(A<RequestData>.Ignored)).Returns(stubResponse);

			new FluentApi<Status>(requestCoordinator).WithParameter("artistId", "123").Please();

			Expression<Func<Response>> callWithArtistId123 =
				() => requestCoordinator.HitEndpoint(A<RequestData>.That.Matches(x => x.Parameters["artistId"] == "123"));

			A.CallTo(callWithArtistId123).MustHaveHappened();
		}

		[Test]
		public void Should_use_custom_http_client()
		{
			var fakeRequestCoordinator = A.Fake<IRequestCoordinator>();
			var fakeHttpClient = new FakeHttpClient();

			new FluentApi<Status>(fakeRequestCoordinator).UsingClient(fakeHttpClient);

			Assert.That(fakeRequestCoordinator.HttpClient, Is.EqualTo(fakeHttpClient));
		}

		[Test]
		public void Should_throw_exception_when_null_client_is_used()
		{
			var fakeRequestCoordinator = A.Fake<IRequestCoordinator>();
			var api = new FluentApi<Status>(fakeRequestCoordinator);

			Assert.Throws<ArgumentNullException>(() => api.UsingClient(null));
		}

		[Test]
		public void should_put_payload_in_action_result()
		{
			var requestCoordinator = new FakeRequestCoordinator { StubPayload = stubResponse };
			var reset = new AutoResetEvent(false);

			new FluentApi<Status>(requestCoordinator)
				.PleaseAsync(
				status =>
				{
					Assert.That(status, Is.Not.Null);
					reset.Set();
				});

			var result = reset.WaitOne(1000 * 60);
			Assert.That(result, Is.True, "Method");
		}

		[Test]
		public void Should_wrap_webexception_under_api_exception_to_be_able_to_know_the_URL()
		{
			const string url = "http://foo.bar.baz/status";

			var requestCoordinator = A.Fake<IRequestCoordinator>();
			A.CallTo(() => requestCoordinator.HitEndpoint(A<RequestData>.Ignored)).Throws<WebException>();
			A.CallTo(() => requestCoordinator.ConstructEndpoint(A<RequestData>.Ignored)).Returns(url);

			var ex = Assert.Throws<ApiWebException>(() => new FluentApi<Status>(requestCoordinator).Please());

			Assert.That(ex.InnerException, Is.Not.Null);
			Assert.That(ex.Uri, Is.EqualTo(url));
			Assert.That(ex.InnerException.GetType(), Is.EqualTo(typeof(WebException)));
		}

		[Test]
		public void Should_throw_exception_when_null_cache_is_used()
		{
			var fakeRequestCoordinator = A.Fake<IRequestCoordinator>();
			var api = new FluentApi<Status>(fakeRequestCoordinator);

			Assert.Throws<ArgumentNullException>(() => api.UsingCache(null));
		}

		[Test]
		public void Should_read_cache()
		{
			var requestCoordinator = A.Fake<IRequestCoordinator>();
			var cache = new FakeCache();
			A.CallTo(() => requestCoordinator.HitEndpoint(A<RequestData>.Ignored))
				.Returns(stubResponse);

			new FluentApi<Status>(requestCoordinator).UsingCache(cache).Please();

			Assert.That(cache.TryGetCount, Is.EqualTo(1));
		}

		[Test]
		public void Should_write_to_cache_on_success()
		{
			var requestCoordinator = A.Fake<IRequestCoordinator>();
			var cache = new FakeCache();
			A.CallTo(() => requestCoordinator.HitEndpoint(A<RequestData>.Ignored))
				.Returns(stubResponse);

			new FluentApi<Status>(requestCoordinator).UsingCache(cache).Please();

			Assert.That(cache.SetCount, Is.EqualTo(1));
			Assert.That(cache.CachedResponses.Count, Is.EqualTo(1));
			Assert.That(cache.CachedResponses[0], Is.EqualTo(stubResponse));
		}

		[Test]
		public void Should_return_value_from_cache()
		{
			var requestCoordinator = A.Fake<IRequestCoordinator>();
			var cache = new FakeCache();
			cache.StubResponse = stubResponse;

			var status =new FluentApi<Status>(requestCoordinator).UsingCache(cache).Please();

			Assert.That(cache.TryGetCount, Is.EqualTo(1));
			Assert.That(status, Is.Not.Null);
		}

		[Test]
		public void Should_not_hit_endpoint_when_value_is_found_in_cache()
		{
			var requestCoordinator = A.Fake<IRequestCoordinator>();
			var cache = new FakeCache();
			cache.StubResponse = stubResponse;

			new FluentApi<Status>(requestCoordinator).UsingCache(cache).Please();
			A.CallTo(() => requestCoordinator.HitEndpoint(A<RequestData>.Ignored)).MustNotHaveHappened();
		}

		[Test]
		public void Should_not_write_to_cache_on_failure()
		{
			var requestCoordinator = A.Fake<IRequestCoordinator>();
			var cache = new FakeCache();
			A.CallTo(() => requestCoordinator.HitEndpoint(A<RequestData>.Ignored)).Throws<WebException>();
			A.CallTo(() => requestCoordinator.ConstructEndpoint(A<RequestData>.Ignored)).Returns("http://foo.com/bar");

			var api = new FluentApi<Status>(requestCoordinator).UsingCache(cache);

			Assert.Throws<ApiWebException>(() => api.Please());

			Assert.That(cache.SetCount, Is.EqualTo(0));
			Assert.That(cache.CachedResponses.Count, Is.EqualTo(0));
		}
	}

	internal class FakeRequestCoordinator : IRequestCoordinator
	{
		public Response HitEndpoint(RequestData requestData)
		{
			throw new NotImplementedException();
		}

		public Response HitEndpointAndGetResponse(RequestData requestData)
		{
			throw new NotImplementedException();
		}

		public void HitEndpointAsync(RequestData requestData, Action<Response> callback)
		{
			callback(StubPayload);
		}

		public string ConstructEndpoint(RequestData requestData)
		{
			throw new NotImplementedException();
		}

		public IHttpClient HttpClient
		{
			get { throw new NotImplementedException(); }
			set { throw new NotImplementedException(); }
		}

		public Response StubPayload { get; set; }
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
