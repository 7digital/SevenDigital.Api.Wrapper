using FakeItEasy;

using NUnit.Framework;

using SevenDigital.Api.Wrapper.EndpointResolution.RequestHandlers;
using SevenDigital.Api.Wrapper.Http;

namespace SevenDigital.Api.Wrapper.Unit.Tests.EndpointResolution.RequestHandlers
{
	[TestFixture]
	public class RequestBuilderMethodTests
	{
		private IRequestBuilder _requestBuilder;
		private IApiUri _apiUri;
		private IOAuthCredentials _oAuthCredentials;

		[SetUp]
		public void Setup()
		{
			_apiUri = A.Fake<IApiUri>();
			A.CallTo(() => _apiUri.Uri).Returns("http://example.com");
			A.CallTo(() => _apiUri.SecureUri).Returns("https://example.com");

			_oAuthCredentials = A.Fake<IOAuthCredentials>();
			A.CallTo(() => _oAuthCredentials.ConsumerKey).Returns("testkey");
			A.CallTo(() => _oAuthCredentials.ConsumerSecret).Returns("testsecret");

			_requestBuilder = new RequestBuilder(_apiUri, _oAuthCredentials);
		}

		[TestCase(HttpMethod.Get)]
		[TestCase(HttpMethod.Post)]
		[TestCase(HttpMethod.Delete)]
		[TestCase(HttpMethod.Put)]
		public void Should_use_correct_http_method(HttpMethod httpMethod)
		{
			var requestData = MakeRequestData(httpMethod, false);

			var request = _requestBuilder.BuildRequest(requestData);

			Assert.That(request.Method, Is.EqualTo(httpMethod));
		}

		[TestCase(HttpMethod.Get)]
		[TestCase(HttpMethod.Post)]
		[TestCase(HttpMethod.Delete)]
		[TestCase(HttpMethod.Put)]
		public void Should_use_non_secure_api_uri_by_default(HttpMethod httpMethod)
		{
			var requestData = MakeRequestData(httpMethod, false);

			var request = _requestBuilder.BuildRequest(requestData);

			Assert.That(request.Url, Is.StringStarting("http://example.com/testpath"));
		}

		[TestCase(HttpMethod.Get)]
		[TestCase(HttpMethod.Post)]
		[TestCase(HttpMethod.Delete)]
		[TestCase(HttpMethod.Put)]
		public void Should_use_secure_uri_when_requested(HttpMethod httpMethod)
		{
			var requestData = MakeRequestData(httpMethod, false);
			requestData.UseHttps = true;
			var request = _requestBuilder.BuildRequest(requestData);

			Assert.That(request.Url, Is.StringStarting("https://example.com/testpath"));
		}

		[TestCase(HttpMethod.Get)]
		[TestCase(HttpMethod.Delete)]
		public void Should_include_parameters_in_querystring(HttpMethod httpMethod)
		{
			var requestData = MakeRequestData(httpMethod, false);
			requestData.Parameters.Add("foo", "bar");

			var request = _requestBuilder.BuildRequest(requestData);

			Assert.That(request.Url, Is.StringContaining("?foo=bar"));
			Assert.That(request.Body, Is.Not.StringContaining("foo=bar"));
		}

		[TestCase(HttpMethod.Post)]
		[TestCase(HttpMethod.Put)]
		public void Should_include_parameters_in_body(HttpMethod httpMethod)
		{
			var requestData = MakeRequestData(httpMethod, false);
			requestData.Parameters.Add("foo", "bar");

			var request = _requestBuilder.BuildRequest(requestData);

			Assert.That(request.Url, Is.Not.StringContaining("foo=bar"));
			Assert.That(request.Body, Is.StringContaining("foo=bar"));
		}

		[TestCase(HttpMethod.Get)]
		[TestCase(HttpMethod.Post)]
		[TestCase(HttpMethod.Delete)]
		[TestCase(HttpMethod.Put)]
		public void Should_substitute_route_parameters_for_supplied_values(HttpMethod httpMethod)
		{
			var requestData = MakeRequestData(httpMethod, false);
			requestData.Parameters.Add("foo", "bar");
			requestData.Endpoint = "test/{foo}/baz";

			var request = _requestBuilder.BuildRequest(requestData);

			Assert.That(request.Url, Is.StringContaining("/test/bar/baz"));
		}

		[TestCase(HttpMethod.Get)]
		[TestCase(HttpMethod.Post)]
		[TestCase(HttpMethod.Delete)]
		[TestCase(HttpMethod.Put)]
		public void Should_not_put_consumer_key_in_url_when_unsigned(HttpMethod httpMethod)
		{
			var requestData = MakeRequestData(httpMethod, false);

			var request = _requestBuilder.BuildRequest(requestData);

			Assert.That(request.Url, Is.Not.StringContaining("oauth"));
		}

		[TestCase(HttpMethod.Get)]
		[TestCase(HttpMethod.Post)]
		[TestCase(HttpMethod.Delete)]
		[TestCase(HttpMethod.Put)]
		public void Should_not_put_consumer_key_in_body_when_unsigned(HttpMethod httpMethod)
		{
			var requestData = MakeRequestData(httpMethod, false);

			var request = _requestBuilder.BuildRequest(requestData);

			Assert.That(request.Body, Is.Not.StringContaining("oauth"));
		}

		[TestCase(HttpMethod.Get)]
		[TestCase(HttpMethod.Post)]
		[TestCase(HttpMethod.Delete)]
		[TestCase(HttpMethod.Put)]
		public void Should_have_auth_header_when_unsigned(HttpMethod httpMethod)
		{
			var requestData = MakeRequestData(httpMethod, false);

			var request = _requestBuilder.BuildRequest(requestData);

			Assert.That(RequestHasAuthHeader(request), Is.True);
		}

		[TestCase(HttpMethod.Get)]
		[TestCase(HttpMethod.Post)]
		[TestCase(HttpMethod.Delete)]
		[TestCase(HttpMethod.Put)]
		public void Should_put_consumer_key_in_auth_header_when_unsigned(HttpMethod httpMethod)
		{
			var requestData = MakeRequestData(httpMethod, false);

			var request = _requestBuilder.BuildRequest(requestData);

			Assert.That(RequestHasAuthHeaderContaining(request, "oauth_consumer_key="), Is.True);
		}

		[TestCase(HttpMethod.Get)]
		[TestCase(HttpMethod.Post)]
		[TestCase(HttpMethod.Delete)]
		[TestCase(HttpMethod.Put)]
		public void Should_not_put_signature_in_auth_header_when_unsigned(HttpMethod httpMethod)
		{
			var requestData = MakeRequestData(httpMethod, false);

			var request = _requestBuilder.BuildRequest(requestData);

			Assert.That(RequestHasAuthHeaderContaining(request, "oauth_signature"), Is.False);
		}

		[TestCase(HttpMethod.Get)]
		[TestCase(HttpMethod.Post)]
		[TestCase(HttpMethod.Delete)]
		[TestCase(HttpMethod.Put)]
		public void Should_not_put_oauth_in_url_when_signed(HttpMethod httpMethod)
		{
			var requestData = MakeRequestData(httpMethod, true);

			var request = _requestBuilder.BuildRequest(requestData);

			Assert.That(request.Url, Is.Not.StringContaining("oauth"));
		}

		[TestCase(HttpMethod.Get)]
		[TestCase(HttpMethod.Post)]
		[TestCase(HttpMethod.Delete)]
		[TestCase(HttpMethod.Put)]
		public void Should_not_put_oauth_in_body_when_signed(HttpMethod httpMethod)
		{
			var requestData = MakeRequestData(httpMethod, true);

			var request = _requestBuilder.BuildRequest(requestData);

			Assert.That(request.Body, Is.Not.StringContaining("oauth"));
		}

		[TestCase(HttpMethod.Get)]
		[TestCase(HttpMethod.Post)]
		[TestCase(HttpMethod.Delete)]
		[TestCase(HttpMethod.Put)]
		public void Should_have_auth_header_when_signed(HttpMethod httpMethod)
		{
			var requestData = MakeRequestData(httpMethod, true);

			var request = _requestBuilder.BuildRequest(requestData);

			Assert.That(RequestHasAuthHeader(request), Is.True);
		}
		
		[TestCase(HttpMethod.Get)]
		[TestCase(HttpMethod.Post)]
		[TestCase(HttpMethod.Delete)]
		[TestCase(HttpMethod.Put)]
		public void Should_sign_with_header_if_required(HttpMethod httpMethod)
		{
			var requestData = MakeRequestData(httpMethod, true);

			var request = _requestBuilder.BuildRequest(requestData);

			Assert.That(RequestHasAuthHeaderContaining(request, "oauth_signature="), Is.True);
			Assert.That(RequestHasAuthHeaderContaining(request, "oauth_consumer_key="), Is.True);
		}

		[TestCase(HttpMethod.Get)]
		[TestCase(HttpMethod.Post)]
		[TestCase(HttpMethod.Delete)]
		[TestCase(HttpMethod.Put)]
		public void Should_include_oauth_token_if_required(HttpMethod httpMethod)
		{
			var requestData = MakeRequestData(httpMethod, true);
			requestData.UserToken = "foo";
			requestData.TokenSecret = "bar";

			var request = _requestBuilder.BuildRequest(requestData);

			Assert.That(RequestHasAuthHeaderContaining(request, "oauth_token=\"foo\""), Is.True);
		}

		private RequestData MakeRequestData(HttpMethod httpMethod, bool requiresSignature)
		{
			return new RequestData
			{
				HttpMethod = httpMethod,
				Endpoint = "testpath",
				RequiresSignature = requiresSignature
			};
		}

		private bool RequestHasAuthHeader(Request request)
		{
			if (!request.Headers.ContainsKey("Authorization"))
			{
				return false;
			}

			var actualHeader = request.Headers["Authorization"];
			return (!string.IsNullOrEmpty(actualHeader));
		}

		private bool RequestHasAuthHeaderContaining(Request request, string expectedHeader)
		{
			if (!request.Headers.ContainsKey("Authorization"))
			{
				return false;
			}

			var actualHeader = request.Headers["Authorization"];
			return (!string.IsNullOrEmpty(actualHeader) && actualHeader.Contains(expectedHeader));
		}
	}
}
