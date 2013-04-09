using System;
using NUnit.Framework;
using SevenDigital.Api.Schema.Basket;

namespace SevenDigital.Api.Wrapper.Integration.Tests.EndpointTests.BasketEndpoint
{
	[TestFixture]
	public class GetPayPalUriTests
	{
		private string _basketId;
		private const int EXPECTED_RELEASE_ID = 160553;

		[TestFixtureSetUp]
		public void SetUp()
		{
			CreateBasket();
			AddReleaseToBasket();
		}

		[Test,Ignore]
		public void Should_get_paypal_url_for_basket()
		{
			PayPalExpressCheckout response = Api<PayPalExpressCheckout>
				.Create
				.UseBasketId(new Guid(_basketId))
				.WithParameter("successUrl", "http://www.google.com")
				.WithParameter("cancelUrl", "http://www.google.com")
				.Please();

			Assert.That(response, Is.Not.Null);
			Assert.That(response.Url.Contains("paypal"), " Paypal Redirect Url {0} does not contain 'paypal'", response.Url);
			Assert.That(response.Url.Contains("token="), " Paypal Redirect Url {0} does not contain 'token='", response.Url);
		}

		private void CreateBasket()
		{
			Basket basketCreate = Api<CreateBasket>.Create
				.WithParameter("country", "GB")
				.Please();
			_basketId = basketCreate.Id;
		}

		private void AddReleaseToBasket()
		{
			Api<AddItemToBasket>.Create
				.UseBasketId(new Guid(_basketId))
				.ForReleaseId(EXPECTED_RELEASE_ID)
				.Please();
		}
	}
}