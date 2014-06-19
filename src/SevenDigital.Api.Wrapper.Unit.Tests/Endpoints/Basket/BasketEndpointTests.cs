using System;
using NUnit.Framework;
using SevenDigital.Api.Schema.Basket;

namespace SevenDigital.Api.Wrapper.Unit.Tests.Endpoints.Basket
{
	[TestFixture]
	public class BasketEndpointTests
	{
		[Test]
		public void Should_not_remove_track_id_parameter_when_adding_a_release_to_basket()
		{
			var basketEndpoint = new FluentApi<AddItemToBasket>();
			basketEndpoint.UseBasketId(NewBasketId()).ForReleaseId(1).ForTrackId(1);
			Assert.That(basketEndpoint.Parameters.Keys.Contains("trackId"));

			basketEndpoint.UseBasketId(NewBasketId());
			Assert.That(basketEndpoint.Parameters.Keys.Contains("releaseId"), Is.True);
			Assert.That(basketEndpoint.Parameters.Keys.Contains("trackId"), Is.True);
		}

		private string NewBasketId()
		{
			return Guid.NewGuid().ToString();
		}
	}
}