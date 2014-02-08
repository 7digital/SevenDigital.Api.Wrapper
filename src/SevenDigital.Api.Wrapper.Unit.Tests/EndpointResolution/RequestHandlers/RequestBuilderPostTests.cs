using FakeItEasy;
using NUnit.Framework;
using SevenDigital.Api.Wrapper.EndpointResolution.RequestHandlers;
using SevenDigital.Api.Wrapper.Http;

namespace SevenDigital.Api.Wrapper.Unit.Tests.EndpointResolution.RequestHandlers
{
	[TestFixture]
	public class RequestBuilderPostTests
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
			var requestData = PostRequestData();

			var request = _builder.BuildRequest(requestData);

			Assert.That(request.Method, Is.EqualTo(HttpMethod.Post));
			Assert.That(request.Url, Is.StringStarting("http://example.com/testpath"));
		}

		[Test]
		public void Should_use_secure_uri_when_requested()
		{
			var requestData = PostRequestData();
			requestData.UseHttps = true;

			var request = _builder.BuildRequest(requestData);

			Assert.That(request.Method, Is.EqualTo(HttpMethod.Post));
			Assert.That(request.Url, Is.StringStarting("https://example.com/testpath"));
		}

		[Test]
		public void Should_not_put_oauth_data_on_uri()
		{
			var requestData = PostRequestData();

			var request = _builder.BuildRequest(requestData);

			Assert.That(request.Method, Is.EqualTo(HttpMethod.Post));

			Assert.That(request.Url, Is.Not.StringContaining("oauth_consumer_key"));
		}

		[Test]
		public void Should_put_oauth_consumer_key_in_headers()
		{
			var requestData = PostRequestData();
			var request = _builder.BuildRequest(requestData);

			Assert.That(request.Method, Is.EqualTo(HttpMethod.Post));
			Assert.That(request.Body, Is.Not.StringContaining("oauth_consumer_key"));
			AssertRequestHasAuthHeaderContaining(request, "oauth_consumer_key");
		}

		[Test]
		public void Should_put_oauth_signature_in_headers()
		{
			var requestData = PostRequestData();
			requestData.RequiresSignature = true;

			var request = _builder.BuildRequest(requestData);

			Assert.That(request.Method, Is.EqualTo(HttpMethod.Post));
			Assert.That(request.Body, Is.Not.StringContaining("oauth_signature"));
			AssertRequestHasAuthHeaderContaining(request, "oauth_signature");
		}

		[Test]
		public void Should_not_put_oauth_signature_in_parameters()
		{
			var requestData = PostRequestData();
			requestData.RequiresSignature = true;

			var request = _builder.BuildRequest(requestData);

			Assert.That(request.Method, Is.EqualTo(HttpMethod.Post));
			Assert.That(request.Url, Is.Not.StringContaining("oauth_signature"));
		}

		[Test]
		public void Should_not_sign_request_by_default()
		{
			var requestData = PostRequestData();

			var request = _builder.BuildRequest(requestData);

			Assert.That(request.Method, Is.EqualTo(HttpMethod.Post));
			AssertRequestHasAuthHeaderNotContaining(request, "oauth_signature");
		}

		[Test]
		public void Should_include_oauth_token_if_required()
		{
			var requestData = PostRequestData();
			requestData.RequiresSignature = true;
			requestData.UserToken = "foo";
			requestData.TokenSecret = "secret";

			var request = _builder.BuildRequest(requestData);

			Assert.That(request.Method, Is.EqualTo(HttpMethod.Post));
			AssertRequestHasAuthHeaderContaining(request, "oauth_token=\"foo\"");
		}

		private static RequestData PostRequestData()
		{
			return new RequestData
			{
				HttpMethod = HttpMethod.Post,
				Endpoint = "testpath",
			};
		}

		private static void AssertRequestHasAuthHeaderContaining(Request request, string expectedContent)
		{
			if (!request.Headers.ContainsKey("Authorization"))
			{
				Assert.Fail("Authorization header not found");
			}

			var headerValue = request.Headers["Authorization"];

			Assert.That(headerValue, Is.StringContaining(expectedContent), "Authorization header does not have expected content");
		}

		private void AssertRequestHasAuthHeaderNotContaining(Request request, string unexepectedContent)
		{
			if (!request.Headers.ContainsKey("Authorization"))
			{
				Assert.Fail("Authorization header not found");
			}

			var headerValue = request.Headers["Authorization"];

			Assert.That(headerValue, Is.Not.StringContaining(unexepectedContent), "Authorization has unexpected content");
		}

	}
}