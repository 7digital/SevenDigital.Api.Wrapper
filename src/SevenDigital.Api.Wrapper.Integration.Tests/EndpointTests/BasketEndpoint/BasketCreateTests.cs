using System;
using System.Linq;
using NUnit.Framework;
using SevenDigital.Api.Schema.Basket;
using SevenDigital.Api.Wrapper.Schema.Basket;

namespace SevenDigital.Api.Wrapper.Integration.Tests.EndpointTests.BasketEndpoint
{
	[TestFixture]
	public class BasketCreateTests
	{
		private const int EXPECTED_RELEASE_ID = 160553;
		private string _basketId;
		private const int EXPECTED_TRACK_ID = 1693930;
		
		[TestFixtureSetUp]
		public void Can_create_basket()
		{
			Basket basketCreate = Api<Basket>.Get
				.Create()
				.WithParameter("country", "GB")
				.Please();

			Assert.That(basketCreate, Is.Not.Null);
			Assert.That(basketCreate.Id, Is.Not.Empty);
			_basketId = basketCreate.Id;
		}

		[Test]
		public void Can_retrieve_that_basket()
		{
			Basket basket = Api<Basket>.Get
				.WithParameter("basketId", _basketId)
				.Please();

			Assert.That(basket, Is.Not.Null);
			Assert.That(basket.Id, Is.EqualTo(_basketId));
		}

		[Test]
		public void Can_add_and_remove_release_to_that_that_basket()
		{
			Basket basket = Api<Basket>.Get
				.AddItem(new Guid(_basketId), EXPECTED_RELEASE_ID)
				.Please();

			Assert.That(basket, Is.Not.Null);
			Assert.That(basket.Id, Is.EqualTo(_basketId));
			Assert.That(basket.BasketItems.Items.Count, Is.GreaterThan(0));
			Assert.That(basket.BasketItems.Items.FirstOrDefault().ReleaseId, Is.EqualTo(EXPECTED_RELEASE_ID.ToString()));

			int toRemove = basket.BasketItems.Items.FirstOrDefault().Id;
			basket = Api<Basket>.Get
				.RemoveItem(new Guid(_basketId), toRemove) 
				.Please();

			Assert.That(basket, Is.Not.Null);
			Assert.That(basket.Id, Is.EqualTo(_basketId));
			Assert.That(basket.BasketItems.Items.Where(x=>x.Id == toRemove).Count(), Is.EqualTo(0));
		}

		[Test]
		public void Can_add_and_remove_track_to_that_that_basket()
		{
			Basket basket = Api<Basket>.Get
								.AddItem(new Guid(_basketId), EXPECTED_RELEASE_ID, EXPECTED_TRACK_ID)
								.Please();

			Assert.That(basket, Is.Not.Null);Assert.That(basket.Id, Is.EqualTo(_basketId));
			Assert.That(basket.BasketItems.Items.Count, Is.GreaterThan(0));
			Assert.That(basket.BasketItems.Items.FirstOrDefault().TrackId, Is.EqualTo(EXPECTED_TRACK_ID.ToString()));

			int toRemove = basket.BasketItems.Items.FirstOrDefault().Id;
			basket = new FluentApi<Basket>()
				.RemoveItem(new Guid(_basketId), toRemove) 
				.Please();

			Assert.That(basket, Is.Not.Null);
			Assert.That(basket.Id, Is.EqualTo(_basketId));
			Assert.That(basket.BasketItems.Items.Where(x => x.Id == toRemove).Count(), Is.EqualTo(0));
		}
	}
}
