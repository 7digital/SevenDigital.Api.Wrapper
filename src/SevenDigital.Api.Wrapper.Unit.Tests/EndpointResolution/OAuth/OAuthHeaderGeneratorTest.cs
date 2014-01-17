using System.Text;
using NUnit.Framework;
using SevenDigital.Api.Wrapper.EndpointResolution.OAuth;
using SevenDigital.Api.Wrapper.EndpointResolution.RequestHandlers;
using SevenDigital.Api.Wrapper.Http;

namespace SevenDigital.Api.Wrapper.Unit.Tests.EndpointResolution.OAuth
{
	[TestFixture]
	public class OAuthHeaderGeneratorTest
	{
		private OAuthHeaderGenerator _oAuthHeaderGenerator;

		[SetUp]
		public void SetUp()
		{
			var appSettingsCredentials = new StubbedConsumerCreds();
			_oAuthHeaderGenerator = new OAuthHeaderGenerator(appSettingsCredentials);
		}

		[Test]
		public void should_return_empty_string_if_endpoint_does_not_require_signature()
		{
			var apiRequest = new ApiRequest
			{
				AbsoluteUrl = "http://foo.com"
			};
			var requestData = new RequestData
			{
				RequiresSignature = false
			};

			var actual = _oAuthHeaderGenerator.GenerateOAuthSignatureHeader(apiRequest, requestData);

			Assert.That(actual, Is.Empty);
		}

		[Test]
		[Description("http://oauth.net/core/1.0/#rfc.section.5.4.1")]
		public void should_return_Authorization_OAuth_headers_as_outlined_in_OAuthspec_if_endpoint_requires_signature_without_usertoken()
		{
			var apiRequest = new ApiRequest
			{
				AbsoluteUrl = "http://foo.com"
			};
			var requestData = new RequestData
			{
				RequiresSignature = true
			};

			var actual = _oAuthHeaderGenerator.GenerateOAuthSignatureHeader(apiRequest, requestData);

			Assert.That(actual, Is.Not.Empty);
			Assert.That(actual, Is.StringStarting("OAuth"));
			Assert.That(actual, Is.StringContaining("oauth_consumer_key="));
			Assert.That(actual, Is.StringContaining("oauth_signature_method="));
			Assert.That(actual, Is.StringContaining("oauth_signature="));
			Assert.That(actual, Is.StringContaining("oauth_timestamp="));
			Assert.That(actual, Is.StringContaining("oauth_nonce="));
			Assert.That(actual, Is.StringContaining("oauth_version="));
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
