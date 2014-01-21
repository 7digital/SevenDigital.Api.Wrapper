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
		[Description("http://oauth.net/core/1.0/#rfc.section.5.4.1")]
		public void should_return_Authorization_OAuth_headers_as_outlined_in_OAuthspec_if_endpoint_requires_signature_without_usertoken()
		{
			var oAuthHeaderData = new OAuthHeaderData
				{
					Url = "http://foo.com"
				};


			var actual = _oAuthHeaderGenerator.GenerateOAuthSignatureHeader(oAuthHeaderData);

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
			var oAuthHeaderData = new OAuthHeaderData
			{
				Url = "http://foo.com"
			};

			var actual = _oAuthHeaderGenerator.GenerateOAuthSignatureHeader(oAuthHeaderData);

			Assert.That(actual, Is.Not.StringContaining("oauth_token="));
		}

		[Test]
		public void should_return_oauth_token_if_Token_provided()
		{
			var oAuthHeaderData = new OAuthHeaderData
			{
				Url = "http://foo.com",
				UserToken = "TOKEN",
				TokenSecret = "SECRET"
			};

			var actual = _oAuthHeaderGenerator.GenerateOAuthSignatureHeader(oAuthHeaderData);
			Assert.That(actual, Is.StringContaining("oauth_token="));

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
