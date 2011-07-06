using System;
using System.Linq;
using NUnit.Framework;
using SevenDigital.Api.Schema.ArtistEndpoint;
using SevenDigital.Api.Schema.Chart;
using SevenDigital.Api.Wrapper.Schema;

namespace SevenDigital.Api.Wrapper.Integration.Tests.EndpointTests.ArtistEndpoint
{
	[TestFixture]
	public class ArtistChartTests
	{
		[Test]
		public void Can_hit_fluent_endpoint()
		{
			var artist = Api<ArtistHasChartPeriod>
							.Get
							.WithToDate(new DateTime(2011, 01, 31))
							.WithPeriod(ChartPeriod.Week)
                            .WithPageSize(20)
							.Please();

			Assert.That(artist, Is.Not.Null);
			Assert.That(artist.ChartItems.Count, Is.EqualTo(20));
			Assert.That(artist.Type, Is.EqualTo(ChartType.artist));
			Assert.That(artist.FromDate, Is.EqualTo(new DateTime(2011, 01, 25)));
			Assert.That(artist.ToDate, Is.EqualTo(new DateTime(2011, 01, 31)));
			Assert.That(artist.ChartItems.FirstOrDefault().Artist, Is.Not.Null);
		}
	}
}