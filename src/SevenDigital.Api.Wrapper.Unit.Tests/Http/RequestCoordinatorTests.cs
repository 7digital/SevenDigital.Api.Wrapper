using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Xml;
using FakeItEasy;
using NUnit.Framework;
using SevenDigital.Api.Wrapper.EndpointResolution;
using SevenDigital.Api.Wrapper.EndpointResolution.RequestHandlers;
using SevenDigital.Api.Wrapper.Http;

namespace SevenDigital.Api.Wrapper.Unit.Tests.Http
{
	[TestFixture]
	public class RequestCoordinatorTests
	{
		private const string API_URL = "http://api.7digital.com/1.2";
		private const string SERVICE_STATUS = "<response status=\"ok\" version=\"1.2\" ><serviceStatus><serverTime>2011-03-04T08:10:29Z</serverTime></serviceStatus></response>";

		private readonly string _consumerKey = new AppSettingsCredentials().ConsumerKey;
		private IHttpClient _httpClient;
		private RequestCoordinator _requestCoordinator;
		private IEnumerable<RequestHandler> _allRequestHandlers;

		[SetUp]
		public void Setup()
		{
			_httpClient = A.Fake<IHttpClient>();
			_allRequestHandlers = RequestHandlerFactory.AllRequestHandlers(EssentialDependencyCheck<IOAuthCredentials>.Instance, EssentialDependencyCheck<IApiUri>.Instance);
			//_allRequestHandlers = new List<RequestHandler>(){A.Fake<RequestHandler>()};
			_requestCoordinator = new RequestCoordinator(_httpClient, _allRequestHandlers);
		}

		[Test]
		public void Should_fire_resolve_with_correct_values()
		{
			A.CallTo(() => _httpClient.Get(A<GetRequest>.Ignored))
				.Returns(new Response(HttpStatusCode.OK, SERVICE_STATUS));

			const string expectedMethod = "GET";
			var expectedHeaders = new Dictionary<string, string>();
			var expected = string.Format("{0}/test?oauth_consumer_key={1}", API_URL, _consumerKey);

			var requestData = new RequestData { Endpoint = "test", HttpMethod = expectedMethod, Headers = expectedHeaders };

			_requestCoordinator.HitEndpoint(requestData);

			A.CallTo(() => _httpClient
					.Get(A<GetRequest>.That.Matches(y => y.Url == expected)))
					.MustHaveHappened();
		}

		[Test]
		public void Should_fire_resolve_with_url_encoded_parameters()
		{
			A.CallTo(() => _httpClient.Get(A<GetRequest>.Ignored))
				.Returns(new Response(HttpStatusCode.OK, SERVICE_STATUS));

			const string unEncodedParameterValue = "Alive & Amplified";

			const string expectedParameterValue = "Alive%20%26%20Amplified";
			var expectedHeaders = new Dictionary<string, string>();
			var testParameters = new Dictionary<string, string> { { "q", unEncodedParameterValue } };
			var expected = string.Format("{0}/test?q={2}&oauth_consumer_key={1}", API_URL, _consumerKey, expectedParameterValue);

			var requestData = new RequestData { Endpoint = "test", HttpMethod = "GET", Headers = expectedHeaders, Parameters = testParameters };

			_requestCoordinator.HitEndpoint(requestData);

			A.CallTo(() => _httpClient
					.Get(A<GetRequest>.That.Matches(y => y.Url == expected)))
					.MustHaveHappened();
		}

		[Test]
		public void Should_not_care_how_many_times_you_create_an_endpoint()
		{
			var endPointState = new RequestData
				{
					Endpoint = "{slug}", 
					HttpMethod = "GET", 
					Parameters = new Dictionary<string, string> { { "slug", "something" } }
				};
			var result = _requestCoordinator.ConstructEndpoint(endPointState);

			Assert.That(result, Is.EqualTo(_requestCoordinator.ConstructEndpoint(endPointState)));
		}

		[Test]
		public void Should_return_xmlnode_if_valid_xml_received()
		{
			Given_a_urlresolver_that_returns_valid_xml();

			var response = _requestCoordinator.HitEndpoint(new RequestData());
			var hitEndpoint = new XmlDocument();
			hitEndpoint.LoadXml(response.Body);
			Assert.That(hitEndpoint.HasChildNodes);
			Assert.That(hitEndpoint.SelectSingleNode("//serverTime"), Is.Not.Null);
		}


		[Test]
		public void Should_return_xmlnode_if_valid_xml_received_using_async()
		{
			var fakeClient = new FakeHttpClient(new Response(HttpStatusCode.OK, SERVICE_STATUS));

			var endpointResolver = new RequestCoordinator(fakeClient, _allRequestHandlers);

			var reset = new AutoResetEvent(false);

			string response = string.Empty;
			endpointResolver.HitEndpointAsync(new RequestData(),
			 s =>
				{
					response = s.Body;
					reset.Set();
				});

			reset.WaitOne(1000 * 5);
			var payload = new XmlDocument();
			payload.LoadXml(response);

			Assert.That(payload.HasChildNodes);
			Assert.That(payload.SelectSingleNode("//serverTime"), Is.Not.Null);
		}

		[Test]
		public void Should_use_api_uri_provided_by_IApiUri_interface()
		{
			const string expectedApiUri = "http://api.7dizzle";

			Given_a_urlresolver_that_returns_valid_xml();

			var apiUri = A.Fake<IApiUri>();

			A.CallTo(() => apiUri.Uri).Returns(expectedApiUri);
			var endpointResolver = new RequestCoordinator(_httpClient, RequestHandlerFactory.AllRequestHandlers(EssentialDependencyCheck<IOAuthCredentials>.Instance, apiUri));

			var requestData = new RequestData
				{
					Endpoint = "test", 
					HttpMethod = "GET", 
					Headers = new Dictionary<string, string>()
				};

			endpointResolver.HitEndpoint(requestData);

			A.CallTo(() => apiUri.Uri).MustHaveHappened(Repeated.Exactly.Once);

			A.CallTo(() => _httpClient.Get(A<GetRequest>.That.Matches(x => x.Url.ToString().Contains(expectedApiUri)))).MustHaveHappened(Repeated.Exactly.Once);
		}

		[Test]
		public void Construct_url_should_combine_url_and_query_params_for_get_requests()
		{
			const string uriPath = "something";
			var result = _requestCoordinator.ConstructEndpoint(new RequestData { Endpoint = uriPath });

			Assert.That(result, Is.EqualTo(API_URL + "/" + uriPath + "?oauth_consumer_key=" + _consumerKey));
		}

		[Test]
		public void Construct_url_should_combine_url_and_not_query_params_for_post_requests()
		{
			const string uriPath = "something";
			var result = _requestCoordinator.ConstructEndpoint(new RequestData { Endpoint = uriPath,HttpMethod = "POST" });

			Assert.That(result, Is.EqualTo(API_URL + "/" + uriPath ));
		}

		private void Given_a_urlresolver_that_returns_valid_xml()
		{
			A.CallTo(() => _httpClient.Get(A<GetRequest>.Ignored))
				.Returns(new Response(HttpStatusCode.OK, SERVICE_STATUS));
		}
	}
}