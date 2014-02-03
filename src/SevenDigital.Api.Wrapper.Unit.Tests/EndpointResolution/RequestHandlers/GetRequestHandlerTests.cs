using System;
using FakeItEasy;
using FakeItEasy.Configuration;
using NUnit.Framework;
using SevenDigital.Api.Wrapper.EndpointResolution.RequestHandlers;
using SevenDigital.Api.Wrapper.Http;

namespace SevenDigital.Api.Wrapper.Unit.Tests.EndpointResolution.RequestHandlers
{
	[TestFixture]
	public class GetRequestHandlerTests
	{
		private IApiUri _apiUri;
		private IOAuthCredentials _oAuthCredentials;
		private IHttpClient _httpClient;

		private GetRequestHandler _handler;
		private RequestData _requestData;

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

			_handler = new GetRequestHandler(_apiUri, _oAuthCredentials);
			_handler.HttpClient = _httpClient;
			_requestData = new RequestData
				{
					HttpMethod = HttpMethod.Get,
					Endpoint = "testpath",
				};
		}

		[Test]
		public void Should_use_non_secure_api_uri_by_default()
		{
			_handler.HitEndpoint(_requestData);

			ARequestToAUriMatching(uri => uri.GetLeftPart(UriPartial.Path) == "http://example.com/testpath").MustHaveHappened();
		}

		[Test]
		public void Should_use_secure_uri_when_requested()
		{
			_requestData.UseHttps = true;

			_handler.HitEndpoint(_requestData);
			
			ARequestToAUriMatching(uri => uri.GetLeftPart(UriPartial.Path) == "https://example.com/testpath").MustHaveHappened();
		}

		[Test]
		public void Should_put_consumer_key_on_constructed_endpoint()
		{
			_requestData.RequiresSignature = false;
			_handler.HitEndpoint(_requestData);

			ARequestToAUriMatching(uri => uri.AbsoluteUri == "http://example.com/testpath?oauth_consumer_key=testkey").MustHaveHappened();
		}

		[Test]
		public void Should_include_parameters_in_querystring()
		{
			_requestData.Parameters.Add("foo", "bar");
			
			_handler.HitEndpoint(_requestData);

			ARequestToAUriMatching(uri => uri.Query.Contains("foo=bar")).MustHaveHappened();
		}

		[Test]
		public void Should_substitute_route_parameters_for_supplied_values()
		{
			_requestData.Parameters.Add("foo", "bar");
			_requestData.Endpoint = "test/{foo}/baz";

			_handler.HitEndpoint(_requestData);

			ARequestToAUriMatching(uri => uri.AbsolutePath == "/test/bar/baz").MustHaveHappened();
		}

		[Test]
		public void Should_not_sign_url_if_not_required()
		{
			_handler.HitEndpoint(_requestData);

			ARequestMatching(HasAuthHeader).MustNotHaveHappened();
		}

		[Test]
		public void Should_sign_url_if_required()
		{
			_requestData.RequiresSignature = true;

			_handler.HitEndpoint(_requestData);
			ARequestMatching(HasAuthHeader).MustHaveHappened();
			ARequestMatching(request => AuthHeaderContaining(request, "oauth_signature")).MustHaveHappened();
		}

		[Test]
		public void Should_include_oauth_token_if_required()
		{
			_requestData.RequiresSignature = true;
			_requestData.UserToken = "foo";
			_requestData.TokenSecret = "bar";
			_handler.HitEndpoint(_requestData);

			ARequestMatching(HasAuthHeader).MustHaveHappened();

			ARequestMatching(r => AuthHeaderContaining(r, "oauth_token=\"foo\"")).MustHaveHappened();
		}

		[Test]
		public void Should_have_oauth_authorization_header()
		{
			_requestData.RequiresSignature = true;

			_handler.HitEndpoint(_requestData);

			ARequestMatching(HasAuthHeader).MustHaveHappened();
		}

		private IAssertConfiguration ARequestToAUriMatching(Func<Uri, bool> predicate)
		{
			return A.CallTo(() => _httpClient.Get(A<GetRequest>.That.Matches(g => predicate(new Uri(g.Url)))));
		}

		private IAssertConfiguration ARequestMatching(Func<GetRequest, bool> predicate)
		{
			return A.CallTo(() => _httpClient.Get(A<GetRequest>.That.Matches(g => predicate(g))));
		}

		private bool HasAuthHeader(GetRequest request)
		{
			if (! request.Headers.ContainsKey("Authorization"))
			{
				return false;
			}

			var actualHeader = request.Headers["Authorization"];
			return (!string.IsNullOrEmpty(actualHeader));
		}
		
		private bool AuthHeaderContaining(GetRequest request, string expectedHeader)
		{
			var actualHeader = request.Headers["Authorization"];
			return (! string.IsNullOrEmpty(actualHeader) && actualHeader.Contains(expectedHeader));
		}

	}
}
