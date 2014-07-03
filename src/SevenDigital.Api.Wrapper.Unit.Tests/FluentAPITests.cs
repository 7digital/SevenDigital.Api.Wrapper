using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FakeItEasy;
using NUnit.Framework;
using SevenDigital.Api.Schema;
using SevenDigital.Api.Schema.Artists;
using SevenDigital.Api.Wrapper.Exceptions;
using SevenDigital.Api.Wrapper.Http;
using SevenDigital.Api.Wrapper.Requests;
using SevenDigital.Api.Wrapper.Requests.Serializing;
using SevenDigital.Api.Wrapper.Responses;
using SevenDigital.Api.Wrapper.Responses.Parsing;
using SevenDigital.Api.Wrapper.Unit.Tests.Http;

namespace SevenDigital.Api.Wrapper.Unit.Tests
{
	[TestFixture]
	public class FluentApiTests
	{
		private const string VALID_STATUS_XML = "<?xml version=\"1.0\" encoding=\"utf-8\" ?><response status=\"ok\" version=\"1.2\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:noNamespaceSchemaLocation=\"http://api.7digital.com/1.2/static/7digitalAPI.xsd\"><serviceStatus><serverTime>2011-05-31T15:31:22Z</serverTime></serviceStatus></response>";

		private readonly Response _stubResponse = new Response(HttpStatusCode.OK, VALID_STATUS_XML);
		private readonly Status _stubStatus = new Status();

		private IRequestBuilder StubRequestBuilder()
		{
			var request = new Request(HttpMethod.Get, "http://example.com/status", new Dictionary<string, string>(), null);
			var requestBuilder = A.Fake<IRequestBuilder>();
			A.CallTo(() => requestBuilder.BuildRequest(A<RequestData>.Ignored)).Returns(request);

			return requestBuilder;
		}

		private IHttpClient StubHttpClient()
		{
			var httpClient = A.Fake<IHttpClient>();
			A.CallTo(() => httpClient.Send(A<Request>.Ignored)).Returns(Task.FromResult(_stubResponse));
			return httpClient;
		}

		private IResponseParser StubResponseParser()
		{
			var responseParser = A.Fake<IResponseParser>();
			return responseParser;
		}

		[Test] 
		public async void Should_call_requestbuilder_with_correct_endpoint_on_resolve()
		{
			var requestBuilder = StubRequestBuilder();
			var httpClient = StubHttpClient();
			var responseParser = StubResponseParser();

			await new FluentApi<Status>(httpClient, requestBuilder, responseParser).Please();

			Expression<Func<Request>> callWithEndpointStatus =
				() => requestBuilder.BuildRequest(A<RequestData>.That.Matches(x => x.Endpoint == "status"));

			A.CallTo(callWithEndpointStatus).MustHaveHappened(Repeated.Exactly.Once);
		}

		[Test]
		public async void Should_fire_requesthandler_with_correct_methodname_on_resolve()
		{
			var requestHandler = StubRequestBuilder();
			var httpClient = StubHttpClient();
			var responseParser = StubResponseParser();

			await new FluentApi<Status>(httpClient, requestHandler, responseParser).WithMethod(HttpMethod.Post).Please();

			Expression<Func<Request>> callWithMethodPost =
				() => requestHandler.BuildRequest(A<RequestData>.That.Matches(x => x.HttpMethod == HttpMethod.Post));

			A.CallTo(callWithMethodPost).MustHaveHappened(Repeated.Exactly.Once);
		}

		[Test]
		public void Should_recongise_standard_http_methods()
		{
			var httpClient = A.Fake<IHttpClient>();
			var requestHandler = A.Fake<IRequestBuilder>();
			var responseParser = A.Fake<IResponseParser>();
			var api = new FluentApi<Status>(httpClient, requestHandler, responseParser);

			api.WithMethod(HttpMethod.Get);
			api.WithMethod(HttpMethod.Post);
			api.WithMethod(HttpMethod.Put);
			api.WithMethod(HttpMethod.Delete);
		}

		[Test]
		public async void Should_fire_requesthandler_with_correct_parameters_on_resolve()
		{
			var requestBuilder = StubRequestBuilder();
			var httpClient = StubHttpClient();
			var responseParser = StubResponseParser();

			var api = new FluentApi<Status>(httpClient, requestBuilder, responseParser);
			await api.WithParameter("artistId", "123").Please();

			Expression<Func<Request>> callWithArtistId123 =
				() => requestBuilder.BuildRequest(A<RequestData>.That.Matches(x => x.Parameters["artistId"] == "123"));

			A.CallTo(callWithArtistId123).MustHaveHappened();
		}

		[Test]
		public async void Should_use_custom_http_client()
		{
			var requestHandler = StubRequestBuilder();
			var httpClient = StubHttpClient();
			var responseParser = StubResponseParser();
			var fakeHttpClient = new FakeHttpClient(_stubResponse);

			var api = new FluentApi<Status>(httpClient, requestHandler, responseParser).UsingClient(fakeHttpClient);
			Assert.That(fakeHttpClient.SendCount, Is.EqualTo(0));

			await api.Please();

			Assert.That(fakeHttpClient.SendCount, Is.EqualTo(1));
		}

		[Test]
		public void Should_throw_exception_when_null_client_is_used()
		{
			var requestHandler = StubRequestBuilder();
			var httpClient = StubHttpClient();
			var responseParser = StubResponseParser();
			var api = new FluentApi<Status>(httpClient, requestHandler, responseParser);

			Assert.Throws<ArgumentNullException>(() => api.UsingClient(null));
		}

		[Test]
		public void Should_wrap_webexception_under_api_exception_to_know_the_url()
		{
			const string url = "http://example.com/status";

			var requestHandler = StubRequestBuilder();
			var httpClient = StubHttpClient();
			var responseParser = StubResponseParser();

			A.CallTo(() => httpClient.Send(A<Request>.Ignored)).Throws<WebException>();

			var api = new FluentApi<Status>(httpClient, requestHandler, responseParser);

			var ex = Assert.Throws<ApiWebException>(async () => await api.Please());

			Assert.That(ex.InnerException, Is.Not.Null);
			Assert.That(ex.OriginalRequest.Url, Is.EqualTo(url));
			Assert.That(ex.InnerException.GetType(), Is.EqualTo(typeof(WebException)));
		}

		[Test]
		public void Should_throw_exception_when_null_cache_is_used()
		{
			var requestHandler = A.Fake<IRequestBuilder>();
			var httpClient = StubHttpClient();
			var responseParser = StubResponseParser();

			var api = new FluentApi<Status>(httpClient, requestHandler, responseParser);

			Assert.Throws<ArgumentNullException>(() => api.UsingCache(null));
		}

		[Test]
		public async void Should_read_cache()
		{
			var httpClient = StubHttpClient();
			var requestHandler = StubRequestBuilder();
			var responseParser = StubResponseParser();

			var api = new FluentApi<Status>(httpClient, requestHandler, responseParser);

			var cache = new FakeCache();
			await api.UsingCache(cache).Please();

			Assert.That(cache.TryGetCount, Is.EqualTo(1));
		}

		[Test]
		public async void Should_write_to_cache_on_success()
		{
			var requestHandler = StubRequestBuilder();
			var httpClient = StubHttpClient();
			var responseParser = StubResponseParser();
			var api = new FluentApi<Status>(httpClient, requestHandler, responseParser);
			
			var cache = new FakeCache();
			await api.UsingCache(cache).Please();

			Assert.That(cache.SetCount, Is.EqualTo(1));
			Assert.That(cache.CachedResponses.Count, Is.EqualTo(1));
			Assert.That(cache.CachedResponses[0], Is.InstanceOf<Status>());
		}

		[Test]
		public async void Should_return_value_from_cache()
		{
			var requestHandler = StubRequestBuilder();
			var httpClient = StubHttpClient();
			var responseParser = StubResponseParser();

			var api = new FluentApi<Status>(httpClient, requestHandler, responseParser);

			var cache = new FakeCache();
			var status = await api.UsingCache(cache).Please();

			Assert.That(cache.TryGetCount, Is.EqualTo(1));
			Assert.That(status, Is.Not.Null);
		}

		[Test]
		public async void Should_not_hit_endpoint_when_value_is_found_in_cache()
		{
			var requestHandler = StubRequestBuilder();
			var httpClient = StubHttpClient();
			var responseParser = StubResponseParser();

			var api = new FluentApi<Status>(httpClient, requestHandler, responseParser);

			var cache = new FakeCache();
			cache.StubCachedObject = _stubStatus;

			await api.UsingCache(cache).Please();
			A.CallTo(() => httpClient.Send(A<Request>.Ignored)).MustNotHaveHappened();
		}

		[Test]
		public void Should_not_write_to_cache_on_failure()
		{
			var requestHandler = StubRequestBuilder();
			var httpClient = StubHttpClient();
			var responseParser = StubResponseParser();

			var api = new FluentApi<Status>(httpClient, requestHandler, responseParser);

			var cache = new FakeCache();

			A.CallTo(() => httpClient.Send(A<Request>.Ignored)).Throws<WebException>();

			api.UsingCache(cache);

			Assert.Throws<ApiWebException>(async () => await api.Please());

			Assert.That(cache.SetCount, Is.EqualTo(0));
			Assert.That(cache.CachedResponses.Count, Is.EqualTo(0));
		}

		[Test]
		public async void Should_allow_you_to_set_a_request_payload()
		{
			var requestBuilder = StubRequestBuilder();
			var httpClient = StubHttpClient();
			var responseParser = StubResponseParser();

			const string payloadData = "{\"hello\":\"world\"}";
			const string payloadContentType = "application/json";

			await new FluentApi<Status>(httpClient, requestBuilder, responseParser).WithPayload(payloadContentType, payloadData).Please();

			Expression<Func<Request>> callWithExpectedPayload = () => requestBuilder.BuildRequest(A<RequestData>.That.Matches(x => x.Payload.ContentType == payloadContentType && x.Payload.Data == payloadData));

			A.CallTo(callWithExpectedPayload).MustHaveHappened();
		}

		[Test]
		public async void Should_allow_you_to_set_a_request_payload_using_an_entity()
		{
			const string expectedXmlOutput = "<?xml version=\"1.0\" encoding=\"utf-8\"?><artist id=\"143451\"><name>MGMT</name><appearsAs>MGMT</appearsAs><image>http://cdn.7static.com/static/img/artistimages/00/001/434/0000143451_150.jpg</image><url>http://www.7digital.com/artist/mgmt/?partner=1401</url></artist>";

			var artist = new Artist
			{
				AppearsAs = "MGMT",
				Name = "MGMT",
				Id = 143451,
				Image = "http://cdn.7static.com/static/img/artistimages/00/001/434/0000143451_150.jpg",
				Url = "http://www.7digital.com/artist/mgmt/?partner=1401"
			};

			var requestBuilder = StubRequestBuilder();
			var httpClient = StubHttpClient();
			var responseParser = StubResponseParser();

			await new FluentApi<Status>(httpClient, requestBuilder, responseParser).WithPayload(artist).Please();

			Expression<Func<Request>> callWithExpectedPayload = () => requestBuilder.BuildRequest(A<RequestData>.That.Matches(x => x.Payload.ContentType == "application/xml" && x.Payload.Data == expectedXmlOutput));

			A.CallTo(callWithExpectedPayload).MustHaveHappened();
		}

		[Test]
		public async void Should_allow_you_to_see_the_raw_response_without_parsing()
		{
			var requestBuilder = StubRequestBuilder();
			var httpClient = StubHttpClient();
			var responseParser = StubResponseParser();

			var response = await new FluentApi<Status>(httpClient, requestBuilder, responseParser).Response();

			Assert.That(response, Is.Not.Null);

			Assert.That(response.Body, Is.Not.Null);
			Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
		}

		[Test]
		public async void Accept_header_defaults_to_xml()
		{
			var requestBuilder = StubRequestBuilder();
			var httpClient = StubHttpClient();
			var responseParser = StubResponseParser();

			await new FluentApi<Status>(httpClient, requestBuilder, responseParser)
				.Please();

			Expression<Func<Request>> callWithExpectedPayload = 
				() => requestBuilder.BuildRequest(A<RequestData>.That.Matches(
					x => x.Accept == "application/xml"));

			A.CallTo(callWithExpectedPayload).MustHaveHappened();
		}

		[Test]
		public async void Should_allow_you_to_set_a_request_payload_using_an_entity_transferred_as_json()
		{
			const string expectedOutput = "{\"id\":143451,\"name\":\"MGMT\",\"sortName\":null,\"appearsAs\":\"MGMT\",\"image\":\"http://cdn.7static.com/static/img/artistimages/00/001/434/0000143451_150.jpg\",\"url\":\"http://www.7digital.com/artist/mgmt/?partner=1401\"}";
			
			var artist = new Artist
			{
				AppearsAs = "MGMT",
				Name = "MGMT",
				Id = 143451,
				Image = "http://cdn.7static.com/static/img/artistimages/00/001/434/0000143451_150.jpg",
				Url = "http://www.7digital.com/artist/mgmt/?partner=1401"
			};

			var requestBuilder = StubRequestBuilder();
			var httpClient = StubHttpClient();
			var responseParser = StubResponseParser();

			await new FluentApi<Status>(httpClient, requestBuilder, responseParser)
				.WithPayload(artist, PayloadFormat.Json)
				.Please();

			Expression<Func<Request>> callWithExpectedPayload = () => requestBuilder.BuildRequest(A<RequestData>.That.Matches(x => x.Payload.ContentType == "application/json" && x.Payload.Data == expectedOutput));

			A.CallTo(callWithExpectedPayload).MustHaveHappened();
		}

		[Test]
		public async void Should_allow_you_to_set_a_request_payload_using_an_entity_transferred_as_xml()
		{
			const string expectedOutput = "<?xml version=\"1.0\" encoding=\"utf-8\"?><artist id=\"143451\"><name>MGMT</name><appearsAs>MGMT</appearsAs><image>http://cdn.7static.com/static/img/artistimages/00/001/434/0000143451_150.jpg</image><url>http://www.7digital.com/artist/mgmt/?partner=1401</url></artist>";

			var artist = new Artist
				{
					AppearsAs = "MGMT",
					Name = "MGMT",
					Id = 143451,
					Image = "http://cdn.7static.com/static/img/artistimages/00/001/434/0000143451_150.jpg",
					Url = "http://www.7digital.com/artist/mgmt/?partner=1401"
				};

			var requestBuilder = StubRequestBuilder();
			var httpClient = StubHttpClient();
			var responseParser = StubResponseParser();

			await new FluentApi<Status>(httpClient, requestBuilder, responseParser)
				.WithPayload(artist, PayloadFormat.Xml)
				.Please();

			Expression<Func<Request>> callWithExpectedPayload = () => requestBuilder.BuildRequest(A<RequestData>.That.Matches(x => x.Payload.ContentType == "application/xml" && x.Payload.Data == expectedOutput));

			A.CallTo(callWithExpectedPayload).MustHaveHappened();
		}
	}
}
