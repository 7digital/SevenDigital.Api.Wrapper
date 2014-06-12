﻿using System.Net.Http;
using NUnit.Framework;
using SevenDigital.Api.Schema.OAuth;

namespace SevenDigital.Api.Wrapper.Integration.Tests.EndpointTests.OAuth
{
	[TestFixture]
	public class OAuthRequestTokenTest
	{
		[Test]
		public async void Should_not_throw_unauthorised_exception_if_correct_creds_passed() 
		{
			var oAuthRequestToken = await Api<OAuthRequestToken>.Create.Please();
			Assert.That(oAuthRequestToken.Secret, Is.Not.Empty);
			Assert.That(oAuthRequestToken.Token, Is.Not.Empty);
		}

		[Test]
		public async void Should_allow_POSTing_to_request_token_endpoint()
		{
			var api = (FluentApi<OAuthRequestToken>) Api<OAuthRequestToken>.Create;

			api.WithMethod(HttpMethod.Post).WithParameter("one", "two");

			var requestToken = await api.Please();

			Assert.That(requestToken.Secret, Is.Not.Empty);
			Assert.That(requestToken.Token, Is.Not.Empty);
		}

		[Test]
		public void POSTing_with_no_data_should_be_allowed()
		{
			var api = (FluentApi<OAuthRequestToken>)Api<OAuthRequestToken>.Create;

			api.WithMethod(HttpMethod.Post);

			Assert.DoesNotThrow(async () => await api.Please());
		}

		[Test]
		public async void Can_handle_odd_characters_in_get_signing_process()
		{
			var oAuthRequestToken = await Api<OAuthRequestToken>
				.Create
				.WithParameter("foo", "%! blah") //arbitrary parameter, but should test for errors in signature generation
				.Please();

			Assert.That(oAuthRequestToken.Secret, Is.Not.Empty);
			Assert.That(oAuthRequestToken.Token, Is.Not.Empty);
		}

		[Test]
		public async void Can_handle_odd_characters_in_post_signing_process()
		{
			var api = (FluentApi<OAuthRequestToken>) Api<OAuthRequestToken>.Create;

			api.WithMethod(HttpMethod.Post);
			api.WithParameter("foo", "%! blah"); //arbitrary parameter, but should test for errors in signature generation

			var oAuthRequestToken = await api.Please();

			Assert.That(oAuthRequestToken.Secret, Is.Not.Empty);
			Assert.That(oAuthRequestToken.Token, Is.Not.Empty);
		}
	}
}
