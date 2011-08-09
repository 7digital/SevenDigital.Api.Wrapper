using System;
using System.Linq;
using NUnit.Framework;
using SevenDigital.Api.Schema.Basket;
using SevenDigital.Api.Wrapper.Extensions.Get;
using SevenDigital.Api.Wrapper.Extensions.Post;

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
			BasketParameters basketParametersCreate = Api<BasketParameters>.Get
				.Create()
				.WithParameter("country", "GB")
				.Please();

			Assert.That(basketParametersCreate, Is.Not.Null);
			Assert.That(basketParametersCreate.Id, Is.Not.Empty);
			_basketId = basketParametersCreate.Id;
		}

		[Test]
		public void Can_retrieve_that_basket()
		{
			BasketParameters basketParameters = Api<BasketParameters>.Get
				.WithParameter("basketId", _basketId)
				.Please();

			Assert.That(basketParameters, Is.Not.Null);
			Assert.That(basketParameters.Id, Is.EqualTo(_basketId));
		}

		[Test]
		public void Can_add_and_remove_release_to_that_that_basket()
		{
			BasketParameters basketParameters = Api<BasketParameters>.Get
				.AddItem(new Guid(_basketId), EXPECTED_RELEASE_ID)
				.Please();

			Assert.That(basketParameters, Is.Not.Null);
			Assert.That(basketParameters.Id, Is.EqualTo(_basketId));
			Assert.That(basketParameters.BasketItems.Items.Count, Is.GreaterThan(0));
			Assert.That(basketParameters.BasketItems.Items.FirstOrDefault().ReleaseId, Is.EqualTo(EXPECTED_RELEASE_ID.ToString()));

			int toRemove = basketParameters.BasketItems.Items.FirstOrDefault().Id;
			basketParameters = Api<BasketParameters>.Get
				.RemoveItem(new Guid(_basketId), toRemove) 
				.Please();

			Assert.That(basketParameters, Is.Not.Null);
			Assert.That(basketParameters.Id, Is.EqualTo(_basketId));
			Assert.That(basketParameters.BasketItems.Items.Where(x=>x.Id == toRemove).Count(), Is.EqualTo(0));
		}

		[Test]
		public void Can_add_and_remove_track_to_that_that_basket()
		{
			BasketParameters basketParameters = Api<BasketParameters>.Get
								.AddItem(new Guid(_basketId), EXPECTED_RELEASE_ID, EXPECTED_TRACK_ID)
								.Please();

			Assert.That(basketParameters, Is.Not.Null);Assert.That(basketParameters.Id, Is.EqualTo(_basketId));
			Assert.That(basketParameters.BasketItems.Items.Count, Is.GreaterThan(0));
			Assert.That(basketParameters.BasketItems.Items.FirstOrDefault().TrackId, Is.EqualTo(EXPECTED_TRACK_ID.ToString()));

			int toRemove = basketParameters.BasketItems.Items.FirstOrDefault().Id;
			basketParameters = new FluentApi<BasketParameters>()
				.RemoveItem(new Guid(_basketId), toRemove) 
				.Please();

			Assert.That(basketParameters, Is.Not.Null);
			Assert.That(basketParameters.Id, Is.EqualTo(_basketId));
			Assert.That(basketParameters.BasketItems.Items.Where(x => x.Id == toRemove).Count(), Is.EqualTo(0));
		}
	}
}
