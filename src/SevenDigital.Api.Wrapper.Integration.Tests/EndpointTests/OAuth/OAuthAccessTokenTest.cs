using System.IO;
using System.Net;
using NUnit.Framework;
using SevenDigital.Api.Schema.OAuth;

namespace SevenDigital.Api.Wrapper.Integration.Tests.EndpointTests.OAuth
{
	[TestFixture]
	public class OAuthAccessTokenTest
	{
		[Test, Ignore("This will need a valid request token and secret!")]
		public void Should_not_throw_unauthorised_exception_if_correct_creds_passed()
		{
			try
			{
				const string oauthToken = "YOUR_REQUEST_TOKEN_HERE";
				const string oauthSecret = "YOUR_TOKEN_SCRET_HERE";
				OAuthAccessToken authAccessToken = Api<OAuthAccessToken>.Create
					.ForUser(oauthToken, oauthSecret)
					.Please();

				Assert.That(authAccessToken.Secret, Is.Not.Empty);
				Assert.That(authAccessToken.Token, Is.Not.Empty);
			}
			catch (WebException ex)
			{
				Assert.Fail(new StreamReader(ex.Response.GetResponseStream()).ReadToEnd());
			}
		}
	}
}