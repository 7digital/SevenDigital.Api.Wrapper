using System;
using System.Collections.Generic;
using System.Threading;
using System.Xml;
using FakeItEasy;
using NUnit.Framework;
using SevenDigital.Api.Wrapper.EndpointResolution;
using SevenDigital.Api.Wrapper.EndpointResolution.OAuth;
using SevenDigital.Api.Wrapper.Utility.Http;

namespace SevenDigital.Api.Wrapper.Unit.Tests.Utility.Http
{
	[TestFixture]
	public class RequestCoordinatorTests
	{
		private const string API_URL = "http://api.7digital.com/1.2";
		private const string SERVICE_STATUS = "<response status=\"ok\" version=\"1.2\" ><serviceStatus><serverTime>2011-03-04T08:10:29Z</serverTime></serviceStatus></response>";
		private readonly string _consumerKey = new AppSettingsCredentials().ConsumerKey;
		private IHttpClient _httpClient;
		private RequestCoordinator _requestCoordinator;
		private IUrlSigner _urlSigner;

		[SetUp]
		public void Setup()
		{
			_httpClient = A.Fake<IHttpClient>();
			_urlSigner = A.Fake<IUrlSigner>();
			_requestCoordinator = new RequestCoordinator(_httpClient, _urlSigner, EssentialDependencyCheck<IOAuthCredentials>.Instance, EssentialDependencyCheck<IApiUri>.Instance);
		}

		[Test]
		public void Should_fire_resolve_with_correct_values()
		{
			A.CallTo(() => _httpClient.Get(A<IRequest>.Ignored.Argument))
				.Returns(new Response<string>{Body = SERVICE_STATUS});

			const string expectedMethod = "GET";
			var expectedHeaders = new Dictionary<string, string>();
			var expected = string.Format("{0}/test?oauth_consumer_key={1}", API_URL, _consumerKey);

			var endPointState = new EndPointInfo { Uri = "test", HttpMethod = expectedMethod, Headers = expectedHeaders };
		

			_requestCoordinator.HitEndpoint(endPointState);

			A.CallTo(() => _httpClient
					.Get(A<IRequest>.That.Matches(y => y.Url == expected).Argument))
					.MustHaveHappened();
		}

		[Test]
		public void Should_fire_resolve_with_url_encoded_parameters()
		{
			A.CallTo(() => _httpClient.Get(A<IRequest>.Ignored.Argument))
				.Returns(new Response<string> {Body = SERVICE_STATUS});

			const string unEncodedParameterValue = "Alive & Amplified";

			const string expectedParameterValue = "Alive%20%26%20Amplified";
			var expectedHeaders = new Dictionary<string, string>();
			var testParameters = new Dictionary<string, string> { { "q", unEncodedParameterValue } };
			var expected = string.Format("{0}/test?oauth_consumer_key={1}&q={2}", API_URL, _consumerKey, expectedParameterValue);

			var endPointState = new EndPointInfo { Uri = "test", HttpMethod = "GET", Headers = expectedHeaders, Parameters = testParameters };

			_requestCoordinator.HitEndpoint(endPointState);

			A.CallTo(() => _httpClient
					.Get(A<IRequest>.That.Matches(y => y.Url == expected).Argument))
					.MustHaveHappened();
		}

		[Test]
		public void Should_not_care_how_many_times_you_create_an_endpoint()
		{
			var endPointState = new EndPointInfo { Uri = "{slug}", HttpMethod = "GET", Parameters =new Dictionary<string, string> { { "slug", "something" } }};
			var result = _requestCoordinator.ConstructEndpoint(endPointState);

			Assert.That(result, Is.EqualTo(_requestCoordinator.ConstructEndpoint(endPointState)));
		}

		[Test]
		public void Should_return_xmlnode_if_valid_xml_received()
		{
			Given_a_urlresolver_that_returns_valid_xml();

			var response = _requestCoordinator.HitEndpoint(new EndPointInfo());
			var hitEndpoint = new XmlDocument();
			hitEndpoint.LoadXml(response);
			Assert.That(hitEndpoint.HasChildNodes);
			Assert.That(hitEndpoint.SelectSingleNode("//serverTime"), Is.Not.Null);
		}


		[Test]
		public void Should_return_xmlnode_if_valid_xml_received_using_async()
		{
			var fakeClient = new FakeHttpClient(new Response<string>() { Body = SERVICE_STATUS });

			var endpointResolver = new RequestCoordinator(fakeClient, _urlSigner, EssentialDependencyCheck<IOAuthCredentials>.Instance, EssentialDependencyCheck<IApiUri>.Instance);

			var reset = new AutoResetEvent(false);

			string response = string.Empty;
			endpointResolver.HitEndpointAsync(new EndPointInfo(),
			 s =>
			 {
				 response = s;
				 reset.Set();
			 });

			reset.WaitOne(1000 * 60);
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

			IOAuthCredentials oAuthCredentials = EssentialDependencyCheck<IOAuthCredentials>.Instance;
			var endpointResolver = new RequestCoordinator(_httpClient, _urlSigner, oAuthCredentials, apiUri);

			var endPointState = new EndPointInfo { Uri = "test", HttpMethod = "GET", Headers = new Dictionary<string, string>() };

			endpointResolver.HitEndpoint(endPointState);

			A.CallTo(() => apiUri.Uri).MustHaveHappened(Repeated.Exactly.Once);

			A.CallTo(() => _httpClient.Get(
				A<IRequest>.That.Matches(x => x.Url.ToString().Contains(expectedApiUri)).Argument))
				.MustHaveHappened(Repeated.Exactly.Once);
		}

		private void Given_a_urlresolver_that_returns_valid_xml()
		{
			A.CallTo(() => _httpClient.Get(A<IRequest>.Ignored.Argument))
				.Returns(new Response<string> { Body = SERVICE_STATUS });
		}
	}

	public class FakeHttpClient : IHttpClient
	{
		private readonly IResponse<string> _fakeResponse;

		public FakeHttpClient(IResponse<string> fakeResponse)
		{
			_fakeResponse = fakeResponse;
		}

		public IResponse<string> Get(IRequest request)
		{
			throw new NotImplementedException();
		}

		public void GetAsync(IRequest request, Action<IResponse<string>> callback)
		{
			callback(_fakeResponse);
		}

		public IResponse<string> Post(IRequest request, string data)
		{
			throw new NotImplementedException();
		}

		public void PostAsync(IRequest request, string data, Action<IResponse<string>> callback)
		{
			throw new NotImplementedException();
		}
	}

}