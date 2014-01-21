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
				OAuthRequestToken oAuthRequestToken = Api<OAuthRequestToken>.Create.Please();
				Assert.That(oAuthRequestToken.Secret, Is.Not.Empty);
				Assert.That(oAuthRequestToken.Token, Is.Not.Empty);
			} 
			catch(WebException ex) 
			{
				Assert.Fail(new StreamReader(ex.Response.GetResponseStream()).ReadToEnd());
			}
		}

		[Test]
		public void Should_allow_POSTing_to_request_token_endpoint()
		{
			try
			{
				var api = (FluentApi<OAuthRequestToken>) Api<OAuthRequestToken>.Create;

				api.WithMethod("POST");

				var requestToken = api.Please();

				Assert.That(requestToken.Secret, Is.Not.Empty);
				Assert.That(requestToken.Token, Is.Not.Empty);
			}
			catch (WebException ex)
			{
				Assert.Fail(new StreamReader(ex.Response.GetResponseStream()).ReadToEnd());
			}
		}

		[Test]
		public void Can_handle_odd_characters_in_get_signing_process()
		{
			try
			{
				OAuthRequestToken oAuthRequestToken = Api<OAuthRequestToken>
					.Create
					.WithParameter("foo", "%! blah") //arbitrary parameter, but should test for errors in signature generation
					.Please();

				Assert.That(oAuthRequestToken.Secret, Is.Not.Empty);
				Assert.That(oAuthRequestToken.Token, Is.Not.Empty);
			}
			catch (WebException ex)
			{
				Assert.Fail(new StreamReader(ex.Response.GetResponseStream()).ReadToEnd());
			}
		}

		[Test]
		public void Can_handle_odd_characters_in_post_signing_process()
		{
			try
			{
				var api = (FluentApi<OAuthRequestToken>) Api<OAuthRequestToken>.Create;

				api.WithMethod("POST");
				api.WithParameter("foo", "%! blah"); //arbitrary parameter, but should test for errors in signature generation

				var oAuthRequestToken = api.Please();

				Assert.That(oAuthRequestToken.Secret, Is.Not.Empty);
				Assert.That(oAuthRequestToken.Token, Is.Not.Empty);
			}
			catch (WebException ex)
			{
				Assert.Fail(new StreamReader(ex.Response.GetResponseStream()).ReadToEnd());
			}
		}
	}
}
