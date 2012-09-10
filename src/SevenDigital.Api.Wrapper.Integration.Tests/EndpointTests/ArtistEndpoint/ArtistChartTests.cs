using System;
using System.Linq;
using NUnit.Framework;
using SevenDigital.Api.Schema.ArtistEndpoint;
using SevenDigital.Api.Schema.Chart;

namespace SevenDigital.Api.Wrapper.Integration.Tests.EndpointTests.ArtistEndpoint
{
	[TestFixture]
	public class ArtistChartTests
	{
		[Test]
		public void Can_hit_fluent_endpoint()
		{
		    var chartDate = DateTime.Today.AddDays(-7);

		    var artist = Api<ArtistChart>
							.Create
							.WithToDate(chartDate)
							.WithPeriod(ChartPeriod.Week)
							.WithPageSize(20)
							.Please();

			Assert.That(artist, Is.Not.Null);
			Assert.That(artist.ChartItems.Count, Is.EqualTo(20));
			Assert.That(artist.Type, Is.EqualTo(ChartType.artist));
			Assert.That(artist.FromDate, Is.LessThanOrEqualTo(chartDate));
			Assert.That(artist.ToDate, Is.GreaterThanOrEqualTo(chartDate));
			Assert.That(artist.ChartItems.FirstOrDefault().Artist, Is.Not.Null);
		}
	}
}