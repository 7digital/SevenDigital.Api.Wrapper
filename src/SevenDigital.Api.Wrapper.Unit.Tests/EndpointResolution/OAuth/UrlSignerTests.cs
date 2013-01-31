using System;
using System.Collections.Generic;
using FakeItEasy;
using NUnit.Framework;
using SevenDigital.Api.Wrapper.EndpointResolution.OAuth;

namespace SevenDigital.Api.Wrapper.Unit.Tests.EndpointResolution.OAuth
{
	[TestFixture]
	public class UrlSignerTests
	{
		private string _consumerKey = "key";
		private string _consumerSecret = "secret";

		[Test]
		public void SignUrlAsString_escapes_those_stupid_plus_signs_and_other_evils_in_signature()
		{
			var url = "http://www.example.com?parameter=hello&again=there";

			for (int i = 0; i < 50; i++)
			{
				var signedUrl = new UrlSigner().SignGetUrl(url, null, null, GetOAuthCredentials());
				var index = signedUrl.IndexOf("oauth_signature");
				var signature = signedUrl.Substring(index + "oauth_signature".Length);

				Assert.That(!signature.Contains("+"), "signature contains a '+' character and isn't being encoded properly");
			}
		}

		[Test]
		public void SignUrl_adds_oauth_signature()
		{
			var url = "http://www.example.com?parameter=hello&again=there";

			var signedUrl = new UrlSigner().SignUrl(url, null, null, GetOAuthCredentials());
			Assert.That(signedUrl.Query.Contains("oauth_signature"));
		}

		[Test]
		public void Make_sure_oath_token_is_encoded_when_POSTing()
		{
			var url = "http://www.example.com/post";

			var signedUrl = new UrlSigner().SignPostRequest(url, "token==", "secret==", GetOAuthCredentials(), new Dictionary<string, string>{{"one", "1"}});

			Assert.That(signedUrl["oauth_token"], Is.EqualTo("token%3D%3D"));
		}

		private IOAuthCredentials GetOAuthCredentials()
		{
			var oAuthCredentials = A.Fake<IOAuthCredentials>();
			A.CallTo(() => oAuthCredentials.ConsumerKey).Returns(_consumerKey);
			A.CallTo(() => oAuthCredentials.ConsumerSecret).Returns(_consumerSecret);
			return oAuthCredentials;
		}
	}
}
