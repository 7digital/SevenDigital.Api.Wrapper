using FakeItEasy;
using NUnit.Framework;
using SevenDigital.Api.Wrapper.EndpointResolution.OAuth;
using SevenDigital.Api.Wrapper.EndpointResolution.RequestHandlers;
using SevenDigital.Api.Wrapper.Http;

namespace SevenDigital.Api.Wrapper.Unit.Tests.EndpointResolution.RequestHandlers
{
	[TestFixture]
	public class GetRequestHandlerTests
	{
		private IApiUri _apiUri;
		private IOAuthCredentials _oAuthCredentials;
		private ISignatureGenerator _signatureGenerator;
		private IHttpClient _httpClient;

		private GetRequestHandler _handler;

		[SetUp]
		public void Setup()
		{
			_apiUri = A.Fake<IApiUri>();
			A.CallTo(() => _apiUri.Uri).Returns("http://testuri.com");
			A.CallTo(() => _apiUri.SecureUri).Returns("https://securetesturi.com");

			_oAuthCredentials = A.Fake<IOAuthCredentials>();
			A.CallTo(() => _oAuthCredentials.ConsumerKey).Returns("testkey");
			A.CallTo(() => _oAuthCredentials.ConsumerSecret).Returns("testsecret");

			_signatureGenerator = A.Fake<ISignatureGenerator>();

			_httpClient = A.Fake<IHttpClient>();

			_handler = new GetRequestHandler(_apiUri, _oAuthCredentials, _signatureGenerator);
			_handler.HttpClient = _httpClient;
		}

		[Test]
		public void Should_construct_api_absolute_path_from_base_uri_and_specified_endpoint()
		{
			var data = GetRequest();

			 _handler.HitEndpoint(data);
			 A.CallTo(() => _httpClient.Get(A<GetRequest>.That.Matches(p => p.Url.StartsWith("http://testuri.com/testpath")))).MustHaveHappened();
		}

		[Test]
		public void Should_use_secure_uri_when_requested()
		{
			var data = GetRequest();
			data.UseHttps = true;

			_handler.HitEndpoint(data);

			A.CallTo(() => _httpClient.Get(A<GetRequest>.That.Matches(p => p.Url.StartsWith("https://securetesturi.com/testpath")))).MustHaveHappened();
		}

		[Test]
		public void Should_not_sign_request_that_does_not_require_it()
		{
			var data = GetRequest();
			data.RequiresSignature = false;

			_handler.HitEndpoint(data);

			A.CallTo(() => _httpClient.Get(A<GetRequest>.That.Matches(p => p.Url.Contains("oauth_signature")))).MustNotHaveHappened();
		}

		[Test]
		public void Should_sign_request_when_required()
		{
			var data = GetRequest();
			data.RequiresSignature = true;

			_handler.HitEndpoint(data);

			A.CallTo(() => _httpClient.Get(A<GetRequest>.That.Matches(p => p.Url.Contains("oauth_signature")))).MustHaveHappened();
		}

		[Test]
		public void Should_sign_request_with_oauth_token_if_provided()
		{
			var data = GetRequest();
			data.RequiresSignature = true;
			data.UserToken = "tokenKey";
			data.TokenSecret = "tokenSecret";

			_handler.HitEndpoint(data);

			A.CallTo(() => _httpClient.Get(A<GetRequest>.That.Matches(p => p.Url.Contains("oauth_token=tokenKey")))).MustHaveHappened();
		}

		private static RequestData GetRequest()
		{
			return new RequestData
			{
				HttpMethod = "GET",
				Endpoint = "testpath",
				UseHttps = false,
			};
		}
	}
}
