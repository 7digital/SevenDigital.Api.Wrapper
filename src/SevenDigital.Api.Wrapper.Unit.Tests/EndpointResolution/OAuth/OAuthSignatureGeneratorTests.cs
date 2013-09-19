using System;
using System.Collections.Generic;
using FakeItEasy;
using NUnit.Framework;
using SevenDigital.Api.Schema.OAuth;
using SevenDigital.Api.Wrapper.EndpointResolution.OAuth;

namespace SevenDigital.Api.Wrapper.Unit.Tests.EndpointResolution.OAuth
{
	[TestFixture]
	public class OAuthSignatureGeneratorTests
	{
		private const string CONSUMER_KEY = "key";
		private const string CONSUMER_SECRET = "secret";

		[Test]
		public void SignUrlAsString_escapes_those_stupid_plus_signs_and_other_evils_in_signature()
		{
			const string url = "http://www.example.com?parameter=hello&again=there";
			var oAuthSignatureInfo = new OAuthSignatureInfo
			{
				FullUrlToSign = url,
				ConsumerCredentials = GetOAuthCredentials(),
				UserAccessToken = new OAuthAccessToken(),
				HttpMethod = "GET"
			};

			for (int i = 0; i < 50; i++)
			{
				var signedUrl = new OAuthSignatureGenerator().Sign(oAuthSignatureInfo);
				var index = signedUrl.IndexOf("oauth_signature");
				var signature = signedUrl.Substring(index + "oauth_signature".Length);

				Assert.That(!signature.Contains("+"), "signature contains a '+' character and isn't being encoded properly");
			}
		}

		[Test]
		public void SignUrlAsString_defaults_to_Get()
		{
			const string url = "http://www.example.com?parameter=hello&again=there";
			var oAuthSignatureInfo = new OAuthSignatureInfo
			{
				FullUrlToSign = url,
				ConsumerCredentials = GetOAuthCredentials(),
				UserAccessToken = new OAuthAccessToken(),
			};

			Assert.That(oAuthSignatureInfo.HttpMethod, Is.EqualTo("GET"));
		}

		[Test]
		public void SignUrl_adds_oauth_signature()
		{
			var url = "http://www.example.com?parameter=hello&again=there";
			var oAuthSignatureInfo = new OAuthSignatureInfo
			{
				FullUrlToSign = url,
				ConsumerCredentials = GetOAuthCredentials(),
				UserAccessToken = new OAuthAccessToken(),
				HttpMethod = "GET"
			};
			var signedUrl = new Uri(new OAuthSignatureGenerator().Sign(oAuthSignatureInfo));
			Assert.That(signedUrl.Query.Contains("oauth_signature"));
		}

		[Test]
		public void Make_sure_oath_token_is_encoded_when_POSTing()
		{
			var url = "http://www.example.com/post";

			var oAuthSignatureInfo = new OAuthSignatureInfo
			{
				FullUrlToSign = url,
				ConsumerCredentials = GetOAuthCredentials(),
				HttpMethod = "POST",
				PostData = new Dictionary<string, string>
				{
					{"one", "1"}
				},
				UserAccessToken = new OAuthAccessToken
				{
					Token = "token==",
					Secret = "secret=="
				}
			};
			var signedUrl = new OAuthSignatureGenerator().SignWithPostData(oAuthSignatureInfo);

			Assert.That(signedUrl["oauth_token"], Is.EqualTo("token%3D%3D"));
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
