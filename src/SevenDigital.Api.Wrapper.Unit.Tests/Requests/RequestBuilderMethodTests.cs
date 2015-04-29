using System.Net.Http;
using FakeItEasy;
using NUnit.Framework;
using SevenDigital.Api.Wrapper.Http;
using SevenDigital.Api.Wrapper.Requests;

namespace SevenDigital.Api.Wrapper.Unit.Tests.Requests
{
	public abstract class RequestBuilderBaseMethodTests
	{
		protected HttpMethod TestedHttpMethod;

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

		[TestCase]
		public void Should_use_correct_http_method()
		{
			var requestData = MakeRequestData(TestedHttpMethod, false);

			var request = _requestBuilder.BuildRequest(requestData);

			Assert.That(request.Method, Is.EqualTo(TestedHttpMethod));
		}

		[TestCase]
		public void Should_use_non_secure_api_uri_by_default()
		{
			var requestData = MakeRequestData(TestedHttpMethod, false);

			var request = _requestBuilder.BuildRequest(requestData);

			Assert.That(request.Url, Is.StringStarting("http://example.com/testpath"));
		}

		[TestCase]
		public void Should_use_secure_uri_when_requested()
		{
			var requestData = MakeRequestData(TestedHttpMethod, false);
			requestData.UseHttps = true;
			var request = _requestBuilder.BuildRequest(requestData);

			Assert.That(request.Url, Is.StringStarting("https://example.com/testpath"));
		}

		[TestCase]
		public void Should_include_parameters_in_querystring()
		{
			if (!TestedHttpMethod.HasParamsInQueryString())
			{
				Assert.Ignore("This http method does not use params in query string");
			}

			var requestData = MakeRequestData(TestedHttpMethod, false);
			requestData.Parameters.Add("foo", "bar");

			var request = _requestBuilder.BuildRequest(requestData);

			Assert.That(request.Url, Is.StringContaining("?foo=bar"));
			Assert.That(request.Body.Data, Is.Empty);
		}

		[TestCase]
		public void Should_include_parameters_in_body()
		{
			if (!TestedHttpMethod.ShouldHaveRequestBody())
			{
				Assert.Ignore("This http method does not use the request body");
			}

			var requestData = MakeRequestData(TestedHttpMethod, false);
			requestData.Parameters.Add("foo", "bar");

			var request = _requestBuilder.BuildRequest(requestData);

			Assert.That(request.Url, Is.Not.StringContaining("foo=bar"));
			Assert.That(request.Body.Data, Is.StringContaining("foo=bar"));
		}

		[TestCase]
		public void Should_include_parameters_in_body_when_signed()
		{
			if (!TestedHttpMethod.ShouldHaveRequestBody())
			{
				Assert.Ignore("This http method does not use the request body");
			}

			var requestData = MakeRequestData(TestedHttpMethod, true);
			requestData.Parameters.Add("foo", "bar");

			var request = _requestBuilder.BuildRequest(requestData);

			Assert.That(request.Url, Is.Not.StringContaining("foo=bar"));
			Assert.That(request.Body.Data, Is.StringContaining("foo=bar"));
		}

		[TestCase]
		public void Should_substitute_route_parameters_for_supplied_values()
		{
			var requestData = MakeRequestData(TestedHttpMethod, false);
			requestData.Parameters.Add("foo", "bar");
			requestData.Endpoint = "test/{foo}/baz";

			var request = _requestBuilder.BuildRequest(requestData);

			Assert.That(request.Url, Is.StringContaining("/test/bar/baz"));
		}

		[TestCase]
		public void Should_not_put_consumer_key_in_url_when_unsigned()
		{
			var requestData = MakeRequestData(TestedHttpMethod, false);

			var request = _requestBuilder.BuildRequest(requestData);

			Assert.That(request.Url, Is.Not.StringContaining("oauth"));
		}

		[TestCase]
		public void Should_not_put_consumer_key_in_body_when_unsigned()
		{
			var requestData = MakeRequestData(TestedHttpMethod, false);

			var request = _requestBuilder.BuildRequest(requestData);

			Assert.That(request.Body.Data, Is.Empty);
		}

		[TestCase]
		public void Should_have_auth_header_when_unsigned()
		{
			var requestData = MakeRequestData(TestedHttpMethod, false);

			var request = _requestBuilder.BuildRequest(requestData);

			Assert.That(RequestHasAuthHeader(request), Is.True);
		}

		[TestCase]
		public void Should_put_consumer_key_in_auth_header_when_unsigned()
		{
			var requestData = MakeRequestData(TestedHttpMethod, false);

			var request = _requestBuilder.BuildRequest(requestData);

			Assert.That(RequestHasAuthHeaderContaining(request, "oauth_consumer_key="), Is.True);
		}

		[TestCase]
		public void Should_not_put_signature_in_auth_header_when_unsigned()
		{
			var requestData = MakeRequestData(TestedHttpMethod, false);

			var request = _requestBuilder.BuildRequest(requestData);

			Assert.That(RequestHasAuthHeaderContaining(request, "oauth_signature"), Is.False);
		}

		[TestCase]
		public void Should_not_put_oauth_in_url_when_signed()
		{
			var requestData = MakeRequestData(TestedHttpMethod, true);

			var request = _requestBuilder.BuildRequest(requestData);

			Assert.That(request.Url, Is.Not.StringContaining("oauth"));
		}

		[TestCase]
		public void Should_not_put_oauth_in_body_when_signed()
		{
			var requestData = MakeRequestData(TestedHttpMethod, true);

			var request = _requestBuilder.BuildRequest(requestData);

			Assert.That(request.Body.Data, Is.Empty);
		}

		[TestCase]
		public void Should_have_auth_header_when_signed()
		{
			var requestData = MakeRequestData(TestedHttpMethod, true);

			var request = _requestBuilder.BuildRequest(requestData);

			Assert.That(RequestHasAuthHeader(request), Is.True);
		}

		[TestCase]
		public void Should_sign_with_header_if_required()
		{
			var requestData = MakeRequestData(TestedHttpMethod, true);

			var request = _requestBuilder.BuildRequest(requestData);

			Assert.That(RequestHasAuthHeaderContaining(request, "oauth_signature="), Is.True);
			Assert.That(RequestHasAuthHeaderContaining(request, "oauth_consumer_key="), Is.True);
		}

		[TestCase]
		public void Should_include_oauth_token_if_required()
		{
			var requestData = MakeRequestData(TestedHttpMethod, true);
			requestData.OAuthToken = "foo";
			requestData.OAuthTokenSecret = "bar";

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

	[TestFixture]
	public class RequestBuilderGetTests : RequestBuilderBaseMethodTests
	{
		public RequestBuilderGetTests()
		{
			TestedHttpMethod = HttpMethod.Get;
		}
	}

	[TestFixture]
	public class RequestBuilderPostTests : RequestBuilderBaseMethodTests
	{
		public RequestBuilderPostTests()
		{
			TestedHttpMethod = HttpMethod.Post;
		}
	}

	[TestFixture]
	public class RequestBuilderPutTests : RequestBuilderBaseMethodTests
	{
		public RequestBuilderPutTests()
		{
			TestedHttpMethod = HttpMethod.Put;
		}
	}

	[TestFixture]
	public class RequestBuilderDeleteTests : RequestBuilderBaseMethodTests
	{
		public RequestBuilderDeleteTests()
		{
			TestedHttpMethod = HttpMethod.Delete;
		}
	}
}
