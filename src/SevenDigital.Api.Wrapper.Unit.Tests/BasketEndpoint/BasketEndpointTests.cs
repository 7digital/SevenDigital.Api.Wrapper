using System;
using NUnit.Framework;
using SevenDigital.Api.Schema.Basket;
using SevenDigital.Api.Wrapper.Extensions.Post;

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
	}
}