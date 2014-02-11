using NUnit.Framework;
using SevenDigital.Api.Wrapper.Requests;

namespace SevenDigital.Api.Wrapper.Unit.Tests.Requests
{
	[TestFixture]
	public class RequestBuilderOAuthHeaderTests
	{
		private RequestBuilder _requestBuilder;

		[SetUp]
		public void SetUp()
		{
			var appSettingsCredentials = new StubbedConsumerCreds();
			var apiUri = new ApiUri();
			_requestBuilder = new RequestBuilder(apiUri, appSettingsCredentials);
		}

		[Test]
		[Description("http://oauth.net/core/1.0/#rfc.section.5.4.1")]
		public void should_return_Authorization_OAuth_headers_as_outlined_in_OAuthspec_if_endpoint_requires_signature_without_usertoken()
		{
			var requestData = new RequestData
				{
					RequiresSignature = true
				};

			var actual = GetAuthHeader(requestData);

			Assert.That(actual, Is.Not.Empty);
			Assert.That(actual, Is.StringStarting("OAuth"));
			Assert.That(actual, Is.StringContaining("oauth_consumer_key="));
			Assert.That(actual, Is.StringContaining("oauth_signature_method="));
			Assert.That(actual, Is.StringContaining("oauth_signature="));
			Assert.That(actual, Is.StringContaining("oauth_timestamp="));
			Assert.That(actual, Is.StringContaining("oauth_nonce="));
			Assert.That(actual, Is.StringContaining("oauth_version="));
		}

		[Test]
		public void should_not_return_oauth_token_if_Token_not_provided()
		{
			var requestData = new RequestData
				{
					RequiresSignature = true
				};

			var actual = GetAuthHeader(requestData);

			Assert.That(actual, Is.Not.StringContaining("oauth_token="));
		}

		[Test]
		public void should_return_oauth_token_if_Token_provided()
		{
			var requestData = new RequestData
				{
					RequiresSignature = true,
					UserToken = "TOKEN",
					TokenSecret = "SECRET"
				};

			var actual = GetAuthHeader(requestData);

			Assert.That(actual, Is.StringContaining("oauth_token="));
		}

		[Test]
		public void should_not_do_oauth_if_not_required()
		{
			var requestData = new RequestData
			{
				RequiresSignature = false
			};

			var actual = GetAuthHeader(requestData);

			Assert.That(actual, Is.Not.StringContaining("oauth_token"));
			Assert.That(actual, Is.StringContaining("oauth_consumer_key="));
		}

		private string GetAuthHeader(RequestData requestData)
		{
			var request = _requestBuilder.BuildRequest(requestData);

			return request.Headers["Authorization"];
		}
	}

	public class StubbedConsumerCreds : IOAuthCredentials
	{
		public string ConsumerKey
		{
			get { return "TOKEN"; }
		}

		public string ConsumerSecret
		{
			get { return "SECRET"; }
		}
	}
}
