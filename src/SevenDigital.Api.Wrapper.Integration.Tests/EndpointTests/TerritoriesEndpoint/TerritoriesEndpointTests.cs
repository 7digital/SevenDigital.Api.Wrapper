using System.Linq;
using NUnit.Framework;
using SevenDigital.Api.Schema.Merchandising;
using SevenDigital.Api.Schema.Territories;

namespace SevenDigital.Api.Wrapper.Integration.Tests.EndpointTests.TerritoriesEndpoint
{
	[TestFixture]
	public class TerritoriesEndpointTests
	{
		[Test, Ignore("In beta testing")]
		public void Can_hit_fluent_endpoint_for_territories()
		{
			var merchList = Api<GeoIpLookup>
				.Get
				.WithIpAddress("84.45.95.241")
				.WithParameter("shopId", "34")
				.Please();

			Assert.That(merchList, Is.Not.Null);
			Assert.That(merchList.CountryCode, Is.EqualTo("7"));
		}
	}
}
