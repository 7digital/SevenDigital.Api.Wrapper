using System.Linq;
using NUnit.Framework;
using SevenDigital.Api.Schema.Territories;

namespace SevenDigital.Api.Wrapper.Integration.Tests.EndpointTests.TerritoriesEndpoint
{
	[TestFixture, Ignore("Permissions not set")]
	public class TerritoriesEndpointTests
	{
		[Test]
		public void Can_hit_fluent_endpoint_for_country_lookup()
		{
			var countryForIp = Api<GeoIpLookup>
				.Create
				.MakeRequest()
				.WithIpAddress("84.45.95.241")
				.WithParameter("shopId", "34")
				.Please();

			Assert.That(countryForIp, Is.Not.Null);
			Assert.That(countryForIp.CountryCode, Is.EqualTo("7"));
		}

		[Test]
		public void Can_hit_fluent_endpoint_for_checkout_restrictions_with_checkout_allowed()
		{
			var restrictions = Api<GeoRestrictions>
				.Create
				.MakeRequest()
				.WithIpAddress("84.45.95.241")
				.WithParameter("shopId", "34")
				.Please();

			Assert.That(restrictions, Is.Not.Null);
			Assert.That(restrictions.AllowCheckout, Is.True);
		}

		[Test]
		public void Can_hit_fluent_endpoint_for_checkout_restrictions_with_checkout_not_allowed()
		{
			var restrictions = Api<GeoRestrictions>
				.Create
				.MakeRequest()
				.WithIpAddress("1.2.3.4")
				.WithParameter("shopId", "34")
				.Please();

			Assert.That(restrictions, Is.Not.Null);
			Assert.That(restrictions.AllowCheckout, Is.False);
		}

		[Test]
		public void Can_hit_fluent_endpoint_for_territories_checkout_restrictions_with_ip_only()
		{
			var restrictions = Api<GeoRestrictions>
				.Create
				.MakeRequest()
				.WithIpAddress("84.45.95.241")
				.Please();

			Assert.That(restrictions, Is.Not.Null);
			Assert.That(restrictions.AllowCheckout, Is.True);
		}

		[Test]
		public void Can_hit_countries_list_endpoint()
		{
			var countries = Api<Countries>
				.Create
				.MakeRequest()
				.Please();

			Assert.That(countries, Is.Not.Null);
			Assert.That(countries.CountryItems, Is.Not.Null);
			Assert.That(countries.CountryItems.Count, Is.GreaterThan(0));
		}

		[Test]
		public void Countries_list_contains_gb()
		{
			var countries = Api<Countries>
				.Create
				.MakeRequest()
				.Please();

			var gb = countries.CountryItems.First(c => c.Code == "GB");
			Assert.That(gb, Is.Not.Null);
			Assert.That(gb.Description, Is.EqualTo("United Kingdom"));
			Assert.That(gb.LocalDescription, Is.EqualTo("United Kingdom"));
			Assert.That(gb.Url, Is.EqualTo("http://www.7digital.com"));
		}

		[Test]
		public void Countries_list_contains_gb_language()
		{
			var countries = Api<Countries>
				.Create
				.MakeRequest()
				.Please();

			var gb = countries.CountryItems.First(c => c.Code == "GB");

			Assert.That(gb.Languages.Count, Is.EqualTo(1));

			var ukEnglish = gb.Languages[0];

			Assert.That(ukEnglish.Description, Is.EqualTo("English"));
			Assert.That(ukEnglish.LocalDescription, Is.EqualTo("English"));
			Assert.That(ukEnglish.Url, Is.EqualTo("http://www.7digital.com"));
		}
	}
}
