using System;
using NUnit.Framework;
using SevenDigital.Api.Schema.Basket;

namespace SevenDigital.Api.Wrapper.Unit.Tests.Endpoints.Basket
{
	[TestFixture]
	public class BasketEndpointTests
	{
		[Test]
		public void Should_remove_track_id_parameter_when_adding_a_release_to_basket()
		{
			var basketEndpoint = new FluentApi<AddItemToBasket>();
            basketEndpoint.UseBasketId(NewBasketId()).ForReleaseId(1).ForTrackId(1);
			Assert.That(basketEndpoint.Parameters.Keys.Contains("trackId"));

            basketEndpoint.UseBasketId(NewBasketId()).ForReleaseId(1);
			Assert.That(basketEndpoint.Parameters.Keys.Contains("trackId"), Is.False);
		}

		[Test]
		public void Should_remove_track_and_release_parameters_when_getting_a_basket()
		{
			var addItemToBasketEndpoint = new FluentApi<AddItemToBasket>();
            addItemToBasketEndpoint.UseBasketId(NewBasketId()).ForReleaseId(1).ForTrackId(1);
			Assert.That(addItemToBasketEndpoint.Parameters.Keys.Contains("trackId"));

			var basketEndpoint = new FluentApi<Schema.Basket.Basket>();
            basketEndpoint.UseBasketId(NewBasketId());

			Assert.That(basketEndpoint.Parameters.Keys.Contains("trackId"), Is.False);
			Assert.That(basketEndpoint.Parameters.Keys.Contains("releaseId"), Is.False);
		}

	    private string NewBasketId()
	    {
	        return Guid.NewGuid().ToString();
	    }
	}
}