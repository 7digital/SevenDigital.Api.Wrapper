using System.Collections.Generic;
using FakeItEasy;
using NUnit.Framework;
using SevenDigital.Api.Wrapper.EndpointResolution.OAuth;

namespace SevenDigital.Api.Wrapper.Unit.Tests.EndpointResolution.OAuth
{
	[TestFixture]
	public class UrlSignerTests
	{
		private ISignatureGenerator _signatureGenerator;
		private UrlSigner _urlSigner;
		private const string CONSUMER_KEY = "key";
		private const string CONSUMER_SECRET = "secret";
		private const string FAKE_URL = "test";
		private const string FAKE_USER_TOKEN = "token";
		private const string FAKE_USER_SECRET = "secret";

		[SetUp]
		public void SetUp()
		{
			_signatureGenerator = A.Fake<ISignatureGenerator>();
			_urlSigner = new UrlSigner(_signatureGenerator);
		}

		[Test]
		public void SignGetUrl_passes_expected_data_to_signature_generator()
		{
			_urlSigner.SignGetUrl(FAKE_URL, FAKE_USER_TOKEN, FAKE_USER_SECRET, GetOAuthCredentials());
			A.CallTo(() => _signatureGenerator.Sign(A<OAuthSignatureInfo>.That.Matches(x => x.HttpMethod == "GET"))).MustHaveHappened();
			A.CallTo(() => _signatureGenerator.Sign(A<OAuthSignatureInfo>.That.Matches(x => x.FullUrlToSign == FAKE_URL))).MustHaveHappened();
			A.CallTo(() => _signatureGenerator.Sign(A<OAuthSignatureInfo>.That.Matches(x => x.UserAccessToken.Token == FAKE_USER_TOKEN))).MustHaveHappened();
			A.CallTo(() => _signatureGenerator.Sign(A<OAuthSignatureInfo>.That.Matches(x => x.UserAccessToken.Secret == FAKE_USER_SECRET))).MustHaveHappened();
		}

		[Test]
		public void SignPostRequest_passes_expected_data_to_signature_generator()
		{
			_urlSigner.SignPostRequest(FAKE_URL, FAKE_USER_TOKEN, FAKE_USER_SECRET, GetOAuthCredentials(), new Dictionary<string, string> { { "one", "1" } });
			A.CallTo(() => _signatureGenerator.SignWithPostData(A<OAuthSignatureInfo>.That.Matches(x => x.HttpMethod == "POST"))).MustHaveHappened();
			A.CallTo(() => _signatureGenerator.SignWithPostData(A<OAuthSignatureInfo>.That.Matches(x => x.FullUrlToSign == FAKE_URL))).MustHaveHappened();
			A.CallTo(() => _signatureGenerator.SignWithPostData(A<OAuthSignatureInfo>.That.Matches(x => x.UserAccessToken.Token == FAKE_USER_TOKEN))).MustHaveHappened();
			A.CallTo(() => _signatureGenerator.SignWithPostData(A<OAuthSignatureInfo>.That.Matches(x => x.UserAccessToken.Secret == FAKE_USER_SECRET))).MustHaveHappened();
		}

		[Test]
		public void SignPostRequest_passes_post_data_to_signature_generator()
		{
			var postParameters = new Dictionary<string, string>
			{
				{"one", "1"}
			};

			_urlSigner.SignPostRequest(FAKE_URL, FAKE_USER_TOKEN, FAKE_USER_SECRET, GetOAuthCredentials(), postParameters);
			A.CallTo(() => _signatureGenerator.SignWithPostData(A<OAuthSignatureInfo>.That.Matches(x => x.PostData == postParameters))).MustHaveHappened();
		}

		private IOAuthCredentials GetOAuthCredentials()
		{
			var oAuthCredentials = A.Fake<IOAuthCredentials>();
			A.CallTo(() => oAuthCredentials.ConsumerKey).Returns(CONSUMER_KEY);
			A.CallTo(() => oAuthCredentials.ConsumerSecret).Returns(CONSUMER_SECRET);
			return oAuthCredentials;
		}
	}
}