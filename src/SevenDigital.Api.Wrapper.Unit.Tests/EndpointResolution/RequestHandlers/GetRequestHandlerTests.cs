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
			A.CallTo(() => _apiUri.Uri).Returns("http://example.com");
			A.CallTo(() => _apiUri.SecureUri).Returns("https://example.com");

			_oAuthCredentials = A.Fake<IOAuthCredentials>();
			A.CallTo(() => _oAuthCredentials.ConsumerKey).Returns("testkey");
			A.CallTo(() => _oAuthCredentials.ConsumerSecret).Returns("testsecret");

			_signatureGenerator = A.Fake<ISignatureGenerator>();

			_httpClient = A.Fake<IHttpClient>();

			_handler = new GetRequestHandler(_apiUri, _oAuthCredentials, _signatureGenerator);
			_handler.HttpClient = _httpClient;
		}

		[Test]
		public void Should_use_non_secure_api_uri_by_default()
		{
			var data = GetRequest();

			_handler.HitEndpoint(data);

			A.CallTo(() => _httpClient.Get(A<GetRequest>.That.Matches(r => r.Url.StartsWith("http://example.com/testpath")))).MustHaveHappened();
		}

		[Test]
		public void Should_use_secure_uri_when_requested()
		{
			var data = GetRequest();
			data.UseHttps = true;

			_handler.HitEndpoint(data);

			A.CallTo(() => _httpClient.Get(A<GetRequest>.That.Matches(r => r.Url.StartsWith("https://example.com/testpath")))).MustHaveHappened();
		}

		[Test]
		public void Should_put_consumer_key_on_constructed_endpoint()
		{
			var data = GetRequest();

			_handler.HitEndpoint(data);

			A.CallTo(() => _oAuthCredentials.ConsumerKey).MustHaveHappened();
			A.CallTo(() => _oAuthCredentials.ConsumerSecret).MustNotHaveHappened();
		}

		[Test]
		public void Should_not_sign_url_if_not_required()
		{
			var data = GetRequest();

			_handler.HitEndpoint(data);

			A.CallTo(() => _signatureGenerator.Sign(A<OAuthSignatureInfo>.Ignored)).MustNotHaveHappened();
		}

		[Test]
		public void Should_sign_url_if_required()
		{
			var data = GetRequest();
			data.RequiresSignature = true;

			_handler.HitEndpoint(data);

			A.CallTo(() => _signatureGenerator.Sign(A<OAuthSignatureInfo>.Ignored)).MustHaveHappened();
		}

		private static RequestData GetRequest()
		{
			return new RequestData
			{
				HttpMethod = "GET",
				Endpoint = "testpath",
				UseHttps = false,
				RequiresSignature = false
			};
		}
	}
}
