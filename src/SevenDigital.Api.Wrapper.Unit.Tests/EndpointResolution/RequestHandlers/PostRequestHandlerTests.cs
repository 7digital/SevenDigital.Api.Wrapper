using FakeItEasy;
using NUnit.Framework;
using SevenDigital.Api.Wrapper.EndpointResolution.RequestHandlers;
using SevenDigital.Api.Wrapper.Http;

namespace SevenDigital.Api.Wrapper.Unit.Tests.EndpointResolution.RequestHandlers
{
	[TestFixture]
	public class PostRequestHandlerTests
	{
		private IApiUri _apiUri;
		private IOAuthCredentials _oAuthCredentials;
		private IHttpClient _httpClient;

		private PostRequestHandler _handler;

		[SetUp]
		public void Setup()
		{
			_apiUri = A.Fake<IApiUri>();
			A.CallTo(() => _apiUri.Uri).Returns("http://example.com");
			A.CallTo(() => _apiUri.SecureUri).Returns("https://example.com");

			_oAuthCredentials = A.Fake<IOAuthCredentials>();
			A.CallTo(() => _oAuthCredentials.ConsumerKey).Returns("testkey");
			A.CallTo(() => _oAuthCredentials.ConsumerSecret).Returns("testsecret");

			_httpClient = A.Fake<IHttpClient>();

			_handler = new PostRequestHandler(_apiUri, _oAuthCredentials);
			_handler.HttpClient = _httpClient;
		}

		[Test]
		public void Should_use_non_secure_api_uri_by_default()
		{
			var data = PostRequest();

			_handler.HitEndpoint(data);

			A.CallTo(() => _httpClient.Post(A<PostRequest>.That.Matches(p => p.Url.StartsWith("http://example.com/testpath")))).MustHaveHappened();
		}

		[Test]
		public void Should_use_secure_uri_when_requested()
		{
			var data = PostRequest();
			data.UseHttps = true;

			_handler.HitEndpoint(data);

			A.CallTo(() => _httpClient.Post(A<PostRequest>.That.Matches(p => p.Url.StartsWith("https://example.com/testpath")))).MustHaveHappened();
		}

		[Test]
		public void Should_not_put_oauth_data_on_uri()
		{
			var data = PostRequest();

			_handler.HitEndpoint(data);

			A.CallTo(() => _httpClient.Post(A<PostRequest>.That.Matches(p => p.Url.Contains("oauth_consumer_key")))).MustNotHaveHappened();
		}

		[Test]
		public void Should_put_oauth_consumer_key_in_headers()
		{
			var data = PostRequest();
			_handler.HitEndpoint(data);
			A.CallTo(() => _httpClient.Post(A<PostRequest>.That.Matches(
				p =>  PostHasAuthHeaderContaining(p, "oauth_consumer_key=testkey")))).MustHaveHappened();
		}

		[Test]
		public void Should_put_oauth_signature_in_headers()
		{
			var data = PostRequest();
			data.RequiresSignature = true;

			_handler.HitEndpoint(data);

			A.CallTo(() => _httpClient.Post(A<PostRequest>.That.Matches(
				p => PostHasAuthHeaderContaining(p, "oauth_signature=")))).MustHaveHappened();
		}


		[Test]
		public void Should_not_put_oauth_signature_in_parameters()
		{
			var data = PostRequest();
			data.RequiresSignature = true;

			_handler.HitEndpoint(data);
			A.CallTo(() => _httpClient.Post(A<PostRequest>.That.Matches(p => p.Body.Contains("oauth_signature=")))).MustNotHaveHappened();
		}

		[Test]
		public void Should_not_sign_constructed_endpoint()
		{
			var data = PostRequest();

			_handler.HitEndpoint(data);
			A.CallTo(() => _httpClient.Post(A<PostRequest>.That.Matches(p => PostHasAuthHeaderContaining(p, "oauth_signature=")))).MustNotHaveHappened();
		}

		[Test]
		public void Should_sign_request_if_required()
		{
			var data = PostRequest();
			data.RequiresSignature = true;

			_handler.HitEndpoint(data);

			A.CallTo(() => _httpClient.Post(A<PostRequest>.That.Matches(
				p => PostHasAuthHeaderContaining(p, "oauth_signature")))).MustHaveHappened();
		}

		[Test]
		public void Should_include_oauth_token_if_required()
		{
			var data = PostRequest();
			data.RequiresSignature = true;
			data.UserToken = "foo";
			data.TokenSecret = "secret";

			_handler.HitEndpoint(data);

			A.CallTo(() => _httpClient.Post(A<PostRequest>.That.Matches(
				p => PostHasAuthHeaderContaining(p, "oauth_token=\"foo\"")))).MustHaveHappened();
		}

		[Test]
		public void Should_use_http_client_to_hit_endpoint()
		{
			var data = PostRequest();

			_handler.HitEndpoint(data);

			A.CallTo(() => _httpClient.Post(A<PostRequest>.Ignored)).MustHaveHappened();
		}

		private static RequestData PostRequest()
		{
			return new RequestData
			{
				HttpMethod = HttpMethod.Post,
				Endpoint = "testpath",
			};
		}

		private static bool PostHasAuthHeaderContaining(PostRequest request, string expectedContent)
		{
			if (!request.Headers.ContainsKey("Authorization"))
			{
				return false;
			}

			var headerValue = request.Headers["Authorization"];

			return headerValue.Contains(expectedContent);
		}
	}
}