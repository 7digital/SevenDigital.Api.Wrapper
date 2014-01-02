using System;
using NUnit.Framework;
using SevenDigital.Api.Schema.Basket;

namespace SevenDigital.Api.Wrapper.Integration.Tests.EndpointTests.BasketEndpoint
{
	[TestFixture]
	public class CompletePayPalExpressCheckoutTests
	{
		[Test, Ignore("Will always fail. Requires a live user who must interact with PayPal prior to this call")]
		public void Should_get_paypal_url_for_basket()
		{
			var userToken = TestDataFromEnvironmentOrAppSettings.AccessToken;
			var userSecret = TestDataFromEnvironmentOrAppSettings.AccessTokenSecret;

			Api<CompletePayPalExpressCheckout>
				.Create
				.UseBasketId(Guid.NewGuid())
				.ForUser(userToken, userSecret)
				.Please();
		}
	}
}