using System;
using System.Linq;
using NUnit.Framework;
using SevenDigital.Api.Schema.Basket;

namespace SevenDigital.Api.Wrapper.Integration.Tests.EndpointTests.BasketEndpoint
{
	[TestFixture]
	public class BasketCreateTests
	{
		private const int EXPECTED_RELEASE_ID = 160553;
		private string _basketId;
		private const int EXPECTED_TRACK_ID = 1693930;

		[TestFixtureSetUp]
		public async void Can_create_basket()
		{
			Basket basketCreate = await Api<CreateBasket>.Create
				.WithParameter("country", "GB")
				.Please();

			Assert.That(basketCreate, Is.Not.Null);
			Assert.That(basketCreate.Id, Is.Not.Empty);
			_basketId = basketCreate.Id;
		}

		[Test]
		public async void Can_retrieve_that_basket()
		{
			Basket basket = await Api<Basket>.Create
				.UseBasketId(new Guid(_basketId))
				.Please();

			Assert.That(basket, Is.Not.Null);
			Assert.That(basket.Id, Is.EqualTo(_basketId));
		}

		[Test]
		public async void Can_add_and_remove_release_to_that_basket()
		{
			Basket basket = await Api<AddItemToBasket>.Create
				.UseBasketId(new Guid(_basketId))
				.ForReleaseId(EXPECTED_RELEASE_ID)
				.Please();

			Assert.That(basket, Is.Not.Null);
			Assert.That(basket.Id, Is.EqualTo(_basketId));
			Assert.That(basket.BasketItems.Items.Count, Is.GreaterThan(0));
			Assert.That(basket.BasketItems.Items.FirstOrDefault().ReleaseId, Is.EqualTo(EXPECTED_RELEASE_ID.ToString()));

			int toRemove = basket.BasketItems.Items.FirstOrDefault().Id;
			basket = await Api<RemoveItemFromBasket>.Create
						.UseBasketId(new Guid(_basketId))
						.BasketItemId(toRemove)
						.Please();

			Assert.That(basket, Is.Not.Null);
			Assert.That(basket.Id, Is.EqualTo(_basketId));
			Assert.That(basket.BasketItems.Items.Count(x => x.Id == toRemove), Is.EqualTo(0));
		}

		[Test]
		public async void Can_add_and_remove_track_to_that_basket()
		{
			Basket basket = await Api<AddItemToBasket>.Create
								.UseBasketId(new Guid(_basketId))
								.ForReleaseId(EXPECTED_RELEASE_ID)
								.ForTrackId(EXPECTED_TRACK_ID)
								.Please();

			Assert.That(basket, Is.Not.Null); Assert.That(basket.Id, Is.EqualTo(_basketId));
			Assert.That(basket.BasketItems.Items.Count, Is.GreaterThan(0));
			Assert.That(basket.BasketItems.Items.FirstOrDefault().TrackId, Is.EqualTo(EXPECTED_TRACK_ID.ToString()));

			int toRemove = basket.BasketItems.Items.FirstOrDefault().Id;
			basket = await Api<RemoveItemFromBasket>.Create
						.UseBasketId(new Guid(_basketId))
						.BasketItemId(toRemove)
						.Please();

			Assert.That(basket, Is.Not.Null);
			Assert.That(basket.Id, Is.EqualTo(_basketId));
			Assert.That(basket.BasketItems.Items.Count(x => x.Id == toRemove), Is.EqualTo(0));
		}

		[Test]
		public async void Should_show_amount_due()
		{
			Basket basket = await Api<AddItemToBasket>.Create
				.UseBasketId(new Guid(_basketId))
				.ForReleaseId(EXPECTED_RELEASE_ID)
				.Please();

			Assert.That(basket.BasketItems.Items.First().AmountDue.Amount, Is.EqualTo("7.99"));
			Assert.That(basket.BasketItems.Items.First().AmountDue.FormattedAmount, Is.EqualTo("£7.99"));
			Assert.That(basket.AmountDue.Amount, Is.EqualTo("7.99"));
			Assert.That(basket.AmountDue.FormattedAmount, Is.EqualTo("£7.99"));
		}

	}
}
