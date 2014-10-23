using System;
using NUnit.Framework;
using SevenDigital.Api.Schema.Releases;

namespace SevenDigital.Api.Wrapper.Integration.Tests.EndpointTests.Releases
{
	[TestFixture]
	[Category("Integration")]
	public class ReleaseDetailsTests
	{
		[Test]
		public async void Can_hit_endpoint()
		{
			var request = Api<Release>.Create
				.ForReleaseId(1685647)
				.WithParameter("country", "GB");
			var release = await request.Please();

			Assert.That(release, Is.Not.Null);
			Assert.That(release.Title, Is.EqualTo("Strangeland"));
			Assert.That(release.Artist.Name, Is.EqualTo("Keane"));
			Assert.That(release.TrackCount, Is.EqualTo(12));
			Assert.That(release.StreamingReleaseDate, Is.EqualTo(DateTime.Parse("2012-05-07")));
			Assert.That(release.Duration, Is.EqualTo(2716));
			Assert.That(release.Price.Currency.Symbol, Is.EqualTo("£"));
            Assert.That(release.Formats.Formats[0].FileFormat, Is.EqualTo("MP3"));

            Assert.That(release.Download.Packages[0].Id, Is.EqualTo(2));
            Assert.That(release.Download.Packages[0].Description, Is.EqualTo("standard"));
            Assert.That(release.Download.Packages[0].PriceResponse.CurrencyCode, Is.EqualTo("GBP"));
            Assert.That(release.Download.Packages[0].PriceResponse.SevendigitalPrice, Is.EqualTo(8.99));
            Assert.That(release.Download.Packages[0].PriceResponse.RecommendedRetailPrice, Is.EqualTo(8.99));
            Assert.That(release.Download.Packages[0].Formats[0].Id, Is.EqualTo((17)));
            Assert.That(release.Download.Packages[0].Formats[0].Description, Is.EqualTo("MP3 320"));
		}
	}
}