using System.Collections.Generic;
using System.Net;
using System.Xml;

using FakeItEasy;

using NUnit.Framework;

using SevenDigital.Api.Wrapper.EndpointResolution;
using SevenDigital.Api.Wrapper.EndpointResolution.RequestHandlers;
using SevenDigital.Api.Wrapper.Http;

namespace SevenDigital.Api.Wrapper.Unit.Tests.EndpointResolution.RequestHandlers
{
	[TestFixture]
	public class RequestHandlerTests
	{
		private const string API_URL = "http://api.7digital.com/1.2";
		private const string SERVICE_STATUS = "<response status=\"ok\" version=\"1.2\" ><serviceStatus><serverTime>2011-03-04T08:10:29Z</serverTime></serviceStatus></response>";

		private readonly string _consumerKey = new AppSettingsCredentials().ConsumerKey;
		private IHttpClient _httpClient;
		private RequestHandler _requestHandler;

		[SetUp]
		public void Setup()
		{
			_httpClient = A.Fake<IHttpClient>();
			_requestHandler = new RequestHandler(EssentialDependencyCheck<IApiUri>.Instance, EssentialDependencyCheck<IOAuthCredentials>.Instance);
			_requestHandler.HttpClient = _httpClient;
		}

		[Test]
		public void Should_fire_resolve_with_correct_values()
		{
			A.CallTo(() => _httpClient.Send(A<Request>.Ignored))
				.Returns(new Response(HttpStatusCode.OK, SERVICE_STATUS));

			const HttpMethod expectedMethod = HttpMethod.Get;
			var expectedHeaders = new Dictionary<string, string>();
			var expected = string.Format("{0}/test?oauth_consumer_key={1}", API_URL, _consumerKey);

			var requestData = new RequestData { Endpoint = "test", HttpMethod = expectedMethod, Headers = expectedHeaders };

			_requestHandler.HitEndpoint(requestData);

			A.CallTo(() => _httpClient
					.Send(A<Request>.That.Matches(y => y.Url == expected)))
					.MustHaveHappened();
		}

		[Test]
		public void Should_fire_resolve_with_url_encoded_parameters()
		{
			A.CallTo(() => _httpClient.Send(A<Request>.Ignored))
				.Returns(new Response(HttpStatusCode.OK, SERVICE_STATUS));

			const string unEncodedParameterValue = "Alive & Amplified";

			const string expectedParameterValue = "Alive%20%26%20Amplified";
			var expectedHeaders = new Dictionary<string, string>();
			var testParameters = new Dictionary<string, string> { { "q", unEncodedParameterValue } };
			var expected = string.Format("{0}/test?q={2}&oauth_consumer_key={1}", API_URL, _consumerKey, expectedParameterValue);

			var requestData = new RequestData
				{
					Endpoint = "test", 
					HttpMethod = HttpMethod.Get, 
					Headers = expectedHeaders, 
					Parameters = testParameters
				};

			_requestHandler.HitEndpoint(requestData);

			A.CallTo(() => _httpClient
					.Send(A<Request>.That.Matches(y => y.Url == expected)))
					.MustHaveHappened();
		}

		[Test]
		public void Should_not_care_how_many_times_you_create_an_endpoint()
		{
			var endPointState = new RequestData
				{
					Endpoint = "{slug}", 
					HttpMethod = HttpMethod.Get, 
					Parameters = new Dictionary<string, string> { { "slug", "something" } }
				};
			var result = _requestHandler.GetDebugUri(endPointState);

			Assert.That(result, Is.EqualTo(_requestHandler.GetDebugUri(endPointState)));
		}

		[Test]
		public void Should_return_xmlnode_if_valid_xml_received()
		{
			Given_a_urlresolver_that_returns_valid_xml();

			var response = _requestHandler.HitEndpoint(new RequestData());
			var hitEndpoint = new XmlDocument();
			hitEndpoint.LoadXml(response.Body);
			Assert.That(hitEndpoint.HasChildNodes);
			Assert.That(hitEndpoint.SelectSingleNode("//serverTime"), Is.Not.Null);
		}

		[Test]
		public void Should_use_api_uri_provided_by_IApiUri_interface()
		{
			const string expectedApiUri = "http://api.7dizzle";

			Given_a_urlresolver_that_returns_valid_xml();

			var apiUri = A.Fake<IApiUri>();

			A.CallTo(() => apiUri.Uri).Returns(expectedApiUri);
			_requestHandler = new RequestHandler(apiUri, EssentialDependencyCheck<IOAuthCredentials>.Instance);
			_requestHandler.HttpClient = _httpClient;

			var requestData = new RequestData
				{
					Endpoint = "test", 
					HttpMethod = HttpMethod.Get, 
					Headers = new Dictionary<string, string>()
				};

			_requestHandler.HitEndpoint(requestData);

			A.CallTo(() => apiUri.Uri).MustHaveHappened(Repeated.Exactly.Once);

			A.CallTo(() => _httpClient.Send(A<Request>.That.Matches(x => x.Url.ToString().Contains(expectedApiUri)))).MustHaveHappened(Repeated.Exactly.Once);
		}

		[Test]
		public void Construct_url_should_combine_url_and_query_params_for_get_requests()
		{
			const string uriPath = "something";
			var request = new RequestData { HttpMethod = HttpMethod.Get, Endpoint = uriPath };
			var result = _requestHandler.GetDebugUri(request);

			Assert.That(result, Is.EqualTo(API_URL + "/" + uriPath + "?oauth_consumer_key=" + _consumerKey));
		}

		[Test]
		public void Construct_url_should_combine_url_and_not_query_params_for_post_requests()
		{
			const string uriPath = "something";
			var request = new RequestData { HttpMethod = HttpMethod.Post, Endpoint = uriPath };
			var result = _requestHandler.GetDebugUri(request);

			Assert.That(result, Is.EqualTo(API_URL + "/" + uriPath ));
		}

		private void Given_a_urlresolver_that_returns_valid_xml()
		{
			A.CallTo(() => _httpClient.Send(A<Request>.Ignored))
				.Returns(new Response(HttpStatusCode.OK, SERVICE_STATUS));
		}
	}
}