using System;
using System.Configuration;
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
			var userToken = ConfigurationManager.AppSettings["Integration.Tests.AccessToken"];
			var userSecret = ConfigurationManager.AppSettings["Integration.Tests.AccessTokenSecret"];

			Api<CompletePayPalExpressCheckout>
				.Create
				.UseBasketId(Guid.NewGuid())
				.ForUser(userToken, userSecret)
				.Please();
		}
	}
}