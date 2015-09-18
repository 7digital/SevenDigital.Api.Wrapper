using System;
using System.Collections.Generic;
using System.Net.Http;
using FakeItEasy;
using NUnit.Framework;
using SevenDigital.Api.Wrapper.Environment;
using SevenDigital.Api.Wrapper.Requests;

namespace SevenDigital.Api.Wrapper.Unit.Tests.Requests
{
	[TestFixture]
	public class RequestBuilderTests
	{
		private const string API_URL = "http://api.7digital.com/1.2";
		private RequestBuilder _requestBuilder;

		[SetUp]
		public void Setup()
		{
			_requestBuilder = new RequestBuilder(
				EssentialDependencyCheck<IApiUri>.Instance,
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
				Parameters = new Dictionary<string, string>
				{
					{"slug", "something"}
				}
			};

			var request1 = _requestBuilder.BuildRequest(endPointState);
			var request2 = _requestBuilder.BuildRequest(endPointState);

			Assert.That(request1.Url, Is.EqualTo(request2.Url));
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

			var request = _requestBuilder.BuildRequest(requestData);

			Assert.That(request.Url, Is.StringStarting(expectedApiUri));
		}

		[Test]
		public void Post_data_with_params_defaults_to_key_value_pair_post_body()
		{
			var parameters = new Dictionary<string, string>
			{
				{"one", "one"}
			};
			var requestData = new RequestData
			{
				Parameters = parameters,
				HttpMethod = HttpMethod.Post
			};

			var request = _requestBuilder.BuildRequest(requestData);
			Assert.That(request.Body.Data, Is.EqualTo(parameters.ToQueryString()));
			Assert.That(request.Body.ContentType, Is.EqualTo("application/x-www-form-urlencoded"));
		}

		[Test]
		public void Post_data_with_params_and_requestBody_defaults_to_key_value_pair_post_body()
		{
			var parameters = new Dictionary<string, string>
			{
				{"one", "one"}
			};
			var requestData = new RequestData
			{
				Parameters = parameters,
				HttpMethod = HttpMethod.Post,
				Payload = new RequestPayload("text/plain", "I am a payload")
			};

			var request = _requestBuilder.BuildRequest(requestData);
			Assert.That(request.Body.Data, Is.EqualTo(parameters.ToQueryString()));
			Assert.That(request.Body.ContentType, Is.EqualTo("application/x-www-form-urlencoded"));
		}

		[Test]
		public void Post_data_with_requestBody_passes_post_body_to_main_request()
		{
			var requestData = new RequestData
			{
				HttpMethod = HttpMethod.Post,
				Payload = new RequestPayload("text/plain", "I am a payload")
			};

			var request = _requestBuilder.BuildRequest(requestData);
			Assert.That(request.Body.Data, Is.EqualTo("I am a payload"));
			Assert.That(request.Body.ContentType, Is.EqualTo("text/plain"));
		}

		[Test]
		public void Should_use_base_uri_when_it_is_present()
		{
			const string expectedApiUri = "http://api.7dizzle";
			var baseUriProvider = A.Fake<IBaseUriProvider>();
			A.CallTo(() => baseUriProvider.BaseUri(A<RequestData>.Ignored)).Returns(expectedApiUri);

			var requestData = new RequestData
			{
				Endpoint = "test",
				HttpMethod = HttpMethod.Get,
				Headers = new Dictionary<string, string>(),
				BaseUriProvider = baseUriProvider
			};

			var request = _requestBuilder.BuildRequest(requestData);

			Assert.That(request.Url, Is.StringStarting(expectedApiUri));
		}

		[Test]
		public void Should_add_traceId_header_as_a_valid_guid()
		{
			const string expectedApiUri = "http://api.7dizzle";
			var baseUriProvider = A.Fake<IBaseUriProvider>();
			A.CallTo(() => baseUriProvider.BaseUri(A<RequestData>.Ignored)).Returns(expectedApiUri);

			var requestData = new RequestData
			{
				Endpoint = "test",
				HttpMethod = HttpMethod.Get,
				Headers = new Dictionary<string, string>(),
				BaseUriProvider = baseUriProvider
			};
			var request = _requestBuilder.BuildRequest(requestData);

			var traceIdHeader = request.Headers["x-7d-traceid"];
			Assert.That(traceIdHeader, Is.Not.Null);
			Assert.DoesNotThrow(() => Guid.Parse(traceIdHeader));
		}
	}
}