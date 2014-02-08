using System.Collections.Generic;
using FakeItEasy;
using NUnit.Framework;

using SevenDigital.Api.Wrapper.EndpointResolution;
using SevenDigital.Api.Wrapper.EndpointResolution.RequestHandlers;
using SevenDigital.Api.Wrapper.Http;

namespace SevenDigital.Api.Wrapper.Unit.Tests.EndpointResolution.RequestHandlers
{
	[TestFixture]
	public class RequestBuilderTests
	{
		private const string API_URL = "http://api.7digital.com/1.2";
		private RequestBuilder _requestBuilder;

		[SetUp]
		public void Setup()
		{
			_requestBuilder = new RequestBuilder(EssentialDependencyCheck<IApiUri>.Instance, 
				EssentialDependencyCheck<IOAuthCredentials>.Instance);
		}

		[Test]
		public void Should_make_url_with_url_encoded_parameters()
		{
			const string unEncodedParameterValue = "Alive & Amplified";
			const string encodedParameterValue = "Alive%20%26%20Amplified";

			var testParameters = new Dictionary<string, string> { { "q", unEncodedParameterValue } };
			var expectedUrl = string.Format("{0}/test?q={1}", API_URL, encodedParameterValue);

			var requestData = new RequestData
				{
					Endpoint = "test", 
					HttpMethod = HttpMethod.Get,
					Parameters = testParameters
				};

			var request = _requestBuilder.BuildRequest(requestData);

			Assert.That(request.Url, Is.EqualTo(expectedUrl));
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

			var result1 = _requestBuilder.BuildRequest(endPointState);
			var result2 = _requestBuilder.BuildRequest(endPointState);

			Assert.That(result1.Url, Is.EqualTo(result2.Url));
		}

		[Test]
		public void Should_use_api_uri_provided_by_IApiUri_interface()
		{
			const string expectedApiUri = "http://api.7dizzle";

			var apiUri = A.Fake<IApiUri>();
			A.CallTo(() => apiUri.Uri).Returns(expectedApiUri);
			_requestBuilder = new RequestBuilder(apiUri, EssentialDependencyCheck<IOAuthCredentials>.Instance);

			var requestData = new RequestData
				{
					Endpoint = "test", 
					HttpMethod = HttpMethod.Get, 
					Headers = new Dictionary<string, string>()
				};

			var response = _requestBuilder.BuildRequest(requestData);

			Assert.That(response.Url, Is.StringContaining(expectedApiUri));
		}
	}
}