using FakeItEasy;
using NUnit.Framework;
using SevenDigital.Api.Wrapper.EndpointResolution.RequestHandlers;
using SevenDigital.Api.Wrapper.Http;

namespace SevenDigital.Api.Wrapper.Unit.Tests.EndpointResolution.RequestHandlers
{
	[TestFixture]
	public class RequestBuilderGetTests
	{
		private IApiUri _apiUri;
		private IOAuthCredentials _oAuthCredentials;

		private RequestBuilder _builder;

		[SetUp]
		public void Setup()
		{
			_apiUri = A.Fake<IApiUri>();
			A.CallTo(() => _apiUri.Uri).Returns("http://example.com");
			A.CallTo(() => _apiUri.SecureUri).Returns("https://example.com");

			_oAuthCredentials = A.Fake<IOAuthCredentials>();
			A.CallTo(() => _oAuthCredentials.ConsumerKey).Returns("testkey");
			A.CallTo(() => _oAuthCredentials.ConsumerSecret).Returns("testsecret");

			_builder = new RequestBuilder(_apiUri, _oAuthCredentials);

		}

		[Test]
		public void Should_use_non_secure_api_uri_by_default()
		{
			var requestData = GetRequestData();
			var request = _builder.BuildRequest(requestData);

			Assert.That(request.Method, Is.EqualTo(HttpMethod.Get));
			Assert.That(request.Url, Is.StringStarting("http://example.com/testpath"));
		}

		[Test]
		public void Should_use_secure_uri_when_requested()
		{
			var requestData = GetRequestData();
			requestData.UseHttps = true;
			var request = _builder.BuildRequest(requestData);

			Assert.That(request.Method, Is.EqualTo(HttpMethod.Get));
			Assert.That(request.Url, Is.StringStarting("https://example.com/testpath"));
		}

		[Test]
		public void Should_put_consumer_key_in_auth_header_when_unsigned()
		{
			var requestData = GetRequestData();
			requestData.RequiresSignature = false;
			var request = _builder.BuildRequest(requestData);

			Assert.That(request.Method, Is.EqualTo(HttpMethod.Get));
			Assert.That(request.Url, Is.StringStarting("http://example.com/testpath"));
			Assert.That(request.Url, Is.Not.StringContaining("oauth_consumer_key"));

			Assert.That(RequestHasAuthHeader(request), Is.True);
			Assert.That(RequestHasAuthHeaderContaining(request, "oauth_signature"), Is.False);
			Assert.That(RequestHasAuthHeaderContaining(request, "oauth_consumer_key="), Is.True);

		}

		[Test]
		public void Should_include_parameters_in_querystring()
		{
			var requestData = GetRequestData();
			requestData.Parameters.Add("foo", "bar");

			var request = _builder.BuildRequest(requestData);

			Assert.That(request.Method, Is.EqualTo(HttpMethod.Get));
			Assert.That(request.Url, Is.StringContaining("?foo=bar"));
		}

		[Test]
		public void Should_substitute_route_parameters_for_supplied_values()
		{
			var requestData = GetRequestData();
			requestData.Parameters.Add("foo", "bar");
			requestData.Endpoint = "test/{foo}/baz";

			var request = _builder.BuildRequest(requestData);

			Assert.That(request.Method, Is.EqualTo(HttpMethod.Get));
			Assert.That(request.Url, Is.StringContaining("/test/bar/baz"));
		}

		[Test]
		public void Should_not_sign_url_if_not_required()
		{
			var requestData = GetRequestData();
			var request = _builder.BuildRequest(requestData);

			Assert.That(request.Method, Is.EqualTo(HttpMethod.Get));

			Assert.That(RequestHasAuthHeader(request), Is.True);
			Assert.That(RequestHasAuthHeaderContaining(request, "oauth_signature"), Is.False);
		}

		[Test]
		public void Should_sign_url_if_required()
		{
			var requestData = GetRequestData();
			requestData.RequiresSignature = true;

			var request = _builder.BuildRequest(requestData);

			Assert.That(request.Method, Is.EqualTo(HttpMethod.Get));
			Assert.That(RequestHasAuthHeaderContaining(request, "oauth_signature"), Is.True);
		}

		[Test]
		public void Should_include_oauth_token_if_required()
		{
			var requestData = GetRequestData();
			requestData.RequiresSignature = true;
			requestData.UserToken = "foo";
			requestData.TokenSecret = "bar";
			var request = _builder.BuildRequest(requestData);

			Assert.That(request.Method, Is.EqualTo(HttpMethod.Get));
			Assert.That(RequestHasAuthHeaderContaining(request, "oauth_token=\"foo\""), Is.True);
		}

		[Test]
		public void Should_have_oauth_authorization_header()
		{
			var requestData = GetRequestData();
			requestData.RequiresSignature = true;

			var request = _builder.BuildRequest(requestData);

			Assert.That(request.Method, Is.EqualTo(HttpMethod.Get));
			Assert.That(RequestHasAuthHeader(request), Is.True);
		}

		private static RequestData GetRequestData()
		{
			return new RequestData
			{
				HttpMethod = HttpMethod.Get,
				Endpoint = "testpath",
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
			return (! string.IsNullOrEmpty(actualHeader) && actualHeader.Contains(expectedHeader));
		}

	}
}
