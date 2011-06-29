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
				var signedUrl = new UrlSigner().SignUrlAsString(url, null, null, GetOAuthCredentials());
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

		private IOAuthCredentials GetOAuthCredentials()
		{
			var oAuthCredentials = A.Fake<IOAuthCredentials>();
			A.CallTo(() => oAuthCredentials.ConsumerKey).Returns(_consumerKey);
			A.CallTo(() => oAuthCredentials.ConsumerKey).Returns(_consumerSecret);
			return oAuthCredentials;
		}
	}
}
