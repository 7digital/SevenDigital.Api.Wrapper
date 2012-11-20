using System.IO;
using System.Net;
using NUnit.Framework;
using SevenDigital.Api.Schema.OAuth;

namespace SevenDigital.Api.Wrapper.Integration.Tests.EndpointTests.OAuth
{
	[TestFixture]
	public class OAuthRequestTokenTest
	{
		[Test]
		public void Should_not_throw_unauthorised_exception_if_correct_creds_passed() 
		{
			try 
			{
				OAuthRequestToken oAuthRequestToken = Api<OAuthRequestToken>.Create.MakeRequest().Please();
				Assert.That(oAuthRequestToken.Secret, Is.Not.Empty);
				Assert.That(oAuthRequestToken.Token, Is.Not.Empty);
			} 
			catch(WebException ex) 
			{
				Assert.Fail(new StreamReader(ex.Response.GetResponseStream()).ReadToEnd());
			}
		}
	}
}
