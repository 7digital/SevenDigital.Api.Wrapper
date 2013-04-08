using System;
using NUnit.Framework;
using SevenDigital.Api.Schema.Basket;
using SevenDigital.Api.Schema.ParameterDefinitions.Get;

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

		[Test]
		public void Should_get_paypal_url_for_basket()
		{
			var response = Api<PayPalRedirectUrl>
				.Create
				.UseBasketId(new Guid(_basketId))
				.WithParameter("successUrl","http://www.google.com")	
				.WithParameter("cancelUrl","http://www.google.com")
				.Please();

			Assert.That(response,Is.Not.Null);
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