using System.Linq;
using NUnit.Framework;
using SevenDigital.Api.Schema.Merchandising;
using SevenDigital.Api.Wrapper.Extensions;

namespace SevenDigital.Api.Wrapper.Integration.Tests.EndpointTests.MerchandisingEndpoint
{
	[TestFixture]
	public class MerchandisingListEndpointTests
	{
		[Test, Ignore("In beta testing")]
		public void Can_hit_fluent_endpoint_for_merchandising()
		{
			var merchList = Api<MerchandisingList>
				.Get
				.WithKey("tabAlbums")
				.WithParameter("shopId", "34")
				.Please();

			Assert.That(merchList, Is.Not.Null);
			Assert.That(merchList.Key, Is.EqualTo("tabAlbums"));
			Assert.That(merchList.MerchandisingItems.Count, Is.GreaterThan(1));

			var merchandisingListItem = merchList.MerchandisingItems.First();
			Assert.That(merchandisingListItem.Release.Title, Is.Not.Null);
		}
	}
}
