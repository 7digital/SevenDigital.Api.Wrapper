using FakeItEasy;
using NUnit.Framework;
using SevenDigital.Api.Wrapper.Environment;
using SevenDigital.Api.Wrapper.Requests;
using System.Collections.Generic;
using System.Net.Http;

namespace SevenDigital.Api.Wrapper.Unit.Tests.Requests
{
	[TestFixture]
	public class RequestBuilderTests
	{
		private const string API_URL = "http://api.7digital.com/1.2";
		private const string TRACE_ID_HEADER_KEY = "x-7d-traceid";
		private RequestBuilder _requestBuilder;

		[SetUp]
		public void Setup()
		{
			_requestBuilder = new RequestBuilder(
				new RouteParamsSubstitutor(new ApiUri()), 
				EssentialDependencyCheck<IOAuthCredentials>.Instance);
		}

		[Test]
		public void Uses_base_uri_when_it_is_present()
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
		public void Uses_api_uri_provided_by_IApiUri_interface()
		{
			const string expectedApiUri = "http://api.7dizzle";

			var apiUri = A.Fake<IApiUri>();
			A.CallTo(() => apiUri.Uri).Returns(expectedApiUri);
			_requestBuilder = new RequestBuilder(new RouteParamsSubstitutor(apiUri), EssentialDependencyCheck<IOAuthCredentials>.Instance);

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
		public void Encodes_url_params()
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
		public void Does_not_care_how_many_times_an_endpoint_is_made()
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
		public void Post_with_payload_passes_payload_in_body()
		{
			var requestData = new RequestData
			{
				HttpMethod = HttpMethod.Post,
				Payload = new RequestPayload("text/plain", "I am a payload")
			};

			var request = _requestBuilder.BuildRequest(requestData);
			Assert.That(request.Url, Is.Not.StringContaining("?"));
			Assert.That(request.Body.Data, Is.EqualTo("I am a payload"));
			Assert.That(request.Body.ContentType, Is.EqualTo("text/plain"));
		}

		[Test]
		public void Post_with_params_and_no_payload_passes_allowed_params_in_query_and_all_params_in_body()
		{
			var requestData = new RequestData
			{
				HttpMethod = HttpMethod.Post,
				Parameters = new Dictionary<string, string>
				{
					{"UserID", "123"},
					{"COUNTRY", "here"},
					{"shopid", "456"},
					{"otherparam", "blah"},
					{"onBehalfOfPartnerId", "789"}
				}
			};

			var request = _requestBuilder.BuildRequest(requestData);
			Assert.That(request.Url, Is.StringEnding("?UserID=123&COUNTRY=here&shopid=456&onBehalfOfPartnerId=789"));
			Assert.That(request.Body.Data, Is.EqualTo("UserID=123&COUNTRY=here&shopid=456&otherparam=blah&onBehalfOfPartnerId=789"));
			Assert.That(request.Body.ContentType, Is.StringContaining("application/x-www-form-urlencoded"));
		}

		[Test]
		public void Post_with_params_and_payload_passes_allowed_params_in_query_and_payload_in_body()
		{
			var requestData = new RequestData
			{
				HttpMethod = HttpMethod.Post,
				Payload = new RequestPayload("text/plain", "I am a payload"),
				Parameters = new Dictionary<string, string>
				{
					{"UserID", "123"},
					{"COUNTRY", "here"},
					{"shopid", "456"},
					{"otherparam", "blah"},
					{"onBehalfOfPartnerId", "789"}
				}
			};

			var request = _requestBuilder.BuildRequest(requestData);
			Assert.That(request.Url, Is.StringEnding("?UserID=123&COUNTRY=here&shopid=456&onBehalfOfPartnerId=789"));
			Assert.That(request.Body.Data, Is.EqualTo(requestData.Payload.Data));
			Assert.That(request.Body.ContentType, Is.EqualTo("text/plain"));
		}

		[Test]
		public void Post_with_params_passes_some_url_params_and_other_allowed_params_in_query()
		{
			var requestData = new RequestData
			{
				HttpMethod = HttpMethod.Post,
				Endpoint = "/a/{userid}/b/{country}/c/{otherparam}",
				Parameters = new Dictionary<string, string>
				{
					{"UserID", "123"},
					{"COUNTRY", "here"},
					{"otherparam", "blah"},
					{"shopid", "456"},
					{"onBehalfOfPartnerId", "789"}
				}
			};

			var request = _requestBuilder.BuildRequest(requestData);
			Assert.That(request.Url, Is.StringEnding("a/123/b/here/c/blah?shopid=456&onBehalfOfPartnerId=789"));
			Assert.That(request.Body.Data, Is.EqualTo("shopid=456&onBehalfOfPartnerId=789"));
			Assert.That(request.Body.ContentType, Is.StringContaining("application/x-www-form-urlencoded"));
		}

		[Test]
		public void Post_with_payload_does_not_add_to_query_string_params()
		{
			var queryStringParameters = new Dictionary<string, string>();
			var requestData = new RequestData
			{
				HttpMethod = HttpMethod.Post,
				Payload = new RequestPayload("application/x-www-form-urlencoded", "foo=bar"),
				RequiresSignature = true
			};

			var credentials = A.Fake<IOAuthCredentials>();
			A.CallTo(() => credentials.ConsumerKey).Returns("MyKey");
			A.CallTo(() => credentials.ConsumerSecret).Returns("MySecret");

			var substitutor = A.Fake<IRouteParamsSubstitutor>();
			A.CallTo(() => substitutor.SubstituteParamsInRequest(requestData))
				.Returns(new ApiRequest
				{
					Parameters = queryStringParameters,
					AbsoluteUrl = "http://www.7digital.com"
				});

			_requestBuilder = new RequestBuilder(substitutor, credentials);
			_requestBuilder.BuildRequest(requestData);

			Assert.That(queryStringParameters.Count, Is.EqualTo(0), "Unexpected query string parameter");
		}

		[Test]
		public void Adds_traceId_header_as_a_valid_guid()
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

			var hasTraceIdHeader = request.Headers.Keys.Contains(TRACE_ID_HEADER_KEY);
			Assert.That(hasTraceIdHeader, Is.False);
		}

		[Test]
		public void Allows_a_custom_traceId_to_be_specified()
		{
			const string expectedApiUri = "http://api.7dizzle";
			var baseUriProvider = A.Fake<IBaseUriProvider>();
			A.CallTo(() => baseUriProvider.BaseUri(A<RequestData>.Ignored)).Returns(expectedApiUri);

			const string customTraceId = "Immagonnatrace4you";

			var requestData = new RequestData
			{
				Endpoint = "test",
				HttpMethod = HttpMethod.Get,
				Headers = new Dictionary<string, string>(),
				BaseUriProvider = baseUriProvider,
				TraceId = customTraceId
			};
			var request = _requestBuilder.BuildRequest(requestData);

			var traceIdHeader = request.Headers[TRACE_ID_HEADER_KEY];
			Assert.That(traceIdHeader, Is.Not.Null);
			Assert.That(traceIdHeader, Is.EqualTo(customTraceId));
		}
	}
}