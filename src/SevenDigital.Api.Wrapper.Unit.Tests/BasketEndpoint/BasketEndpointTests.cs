using System;
using NUnit.Framework;
using SevenDigital.Api.Schema.Basket;

namespace SevenDigital.Api.Wrapper.Unit.Tests.BasketEndpoint
{
	[TestFixture]
	public class BasketEndpointTests
	{
		[Test]
		public void Should_remove_track_id_parameter_when_adding_a_release_to_basket()
		{
			var basketEndpoint = new FluentApi<Basket>();
			basketEndpoint.AddItem(Guid.NewGuid(),1,1);
			Assert.That(basketEndpoint.Parameters.Keys.Contains("trackId"));
			basketEndpoint.AddItem(Guid.NewGuid(),1);
			Assert.That(basketEndpoint.Parameters.Keys.Contains("trackId"),Is.False);
		}


		[Test]
		public void Should_remove_track_and_release_parameters_when_getting_a_basket()
		{
			var basketEndpoint = new FluentApi<Basket>();
			basketEndpoint.AddItem(Guid.NewGuid(), 1, 1);
			Assert.That(basketEndpoint.Parameters.Keys.Contains("trackId"));
			basketEndpoint.GetBasket(Guid.NewGuid());
			Assert.That(basketEndpoint.Parameters.Keys.Contains("trackId"), Is.False);
			Assert.That(basketEndpoint.Parameters.Keys.Contains("releaseId"), Is.False);
		}
	}
}