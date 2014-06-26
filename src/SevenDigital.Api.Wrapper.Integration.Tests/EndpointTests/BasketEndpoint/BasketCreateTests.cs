using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using SevenDigital.Api.Schema.Baskets;

namespace SevenDigital.Api.Wrapper.Integration.Tests.EndpointTests.BasketEndpoint
{
	[TestFixture]
	public class BasketCreateTests
	{
		private const int EXPECTED_RELEASE_ID = 160553;
		private const int EXPECTED_TRACK_ID = 1693930;

		[Test]
		public async void Can_retrieve_a_basket()
		{
			var basketId = await MakeBasket();

			var request = Api<Basket>.Create
				.UseBasketId(basketId);
			var basket = await request.Please();

			Assert.That(basket, Is.Not.Null);
			Assert.That(basket.Id, Is.EqualTo(basketId));
		}

		[Test]
		public async void Can_add_and_remove_release_to_a_basket()
		{
			var basketId = await MakeBasket();

			var request = Api<AddItemToBasket>.Create
				.UseBasketId(basketId)
				.ForReleaseId(EXPECTED_RELEASE_ID);
			var basketAdded = await request.Please();

			Assert.That(basketAdded, Is.Not.Null);
			Assert.That(basketAdded.Id, Is.EqualTo(basketId));
			Assert.That(basketAdded.BasketItems.Items.Count, Is.GreaterThan(0));
			Assert.That(basketAdded.BasketItems.Items.FirstOrDefault().ReleaseId, Is.EqualTo(EXPECTED_RELEASE_ID.ToString()));

			int toRemove = basketAdded.BasketItems.Items.FirstOrDefault().Id;

			var removeRequest = Api<RemoveItemFromBasket>.Create
				.UseBasketId(basketId)
				.BasketItemId(toRemove);
			var basketRemoved = await removeRequest.Please();

			Assert.That(basketRemoved, Is.Not.Null);
			Assert.That(basketRemoved.Id, Is.EqualTo(basketId));
			Assert.That(basketRemoved.BasketItems.Items.Count(x => x.Id == toRemove), Is.EqualTo(0));
		}

		[Test]
		public async void Can_add_and_remove_track_to_a_basket()
		{
			var basketId = await MakeBasket();

			var addRequest = Api<AddItemToBasket>.Create
				.UseBasketId(basketId)
				.ForReleaseId(EXPECTED_RELEASE_ID)
				.ForTrackId(EXPECTED_TRACK_ID);
			var basketAdded = await addRequest.Please();

			Assert.That(basketAdded, Is.Not.Null); Assert.That(basketAdded.Id, Is.EqualTo(basketId));
			Assert.That(basketAdded.BasketItems.Items.Count, Is.GreaterThan(0));
			Assert.That(basketAdded.BasketItems.Items.FirstOrDefault().TrackId, Is.EqualTo(EXPECTED_TRACK_ID.ToString()));

			int toRemove = basketAdded.BasketItems.Items.FirstOrDefault().Id;

			var request = Api<RemoveItemFromBasket>.Create
				.UseBasketId(basketId)
				.BasketItemId(toRemove);
			var basketRemoved = await request.Please();

			Assert.That(basketRemoved, Is.Not.Null);
			Assert.That(basketRemoved.Id, Is.EqualTo(basketId));
			Assert.That(basketRemoved.BasketItems.Items.Count(x => x.Id == toRemove), Is.EqualTo(0));
		}

		[Test]
		public async void Should_show_amount_due()
		{
			var basketId = await MakeBasket();

			var request = Api<AddItemToBasket>.Create
				.UseBasketId(basketId)
				.ForReleaseId(EXPECTED_RELEASE_ID);
			var basket = await request.Please();

			Assert.That(basket.BasketItems.Items.First().AmountDue.Amount, Is.EqualTo("7.99"));
			Assert.That(basket.BasketItems.Items.First().AmountDue.FormattedAmount, Is.EqualTo("£7.99"));
			Assert.That(basket.AmountDue.Amount, Is.EqualTo("7.99"));
			Assert.That(basket.AmountDue.FormattedAmount, Is.EqualTo("£7.99"));
		}

		private async Task<string> MakeBasket()
		{
			var createBasketRequest = Api<CreateBasket>.Create
				.WithParameter("country", "GB");
			var basketCreate = await createBasketRequest.Please();

			Assert.That(basketCreate, Is.Not.Null);
			Assert.That(basketCreate.Id, Is.Not.Empty);
			
			return basketCreate.Id;
		}
	}
}
