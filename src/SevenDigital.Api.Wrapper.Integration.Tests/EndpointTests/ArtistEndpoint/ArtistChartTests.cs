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
		public async void Can_hit_fluent_endpoint()
		{
			var chartDate = DateTime.Today.AddDays(-7);

			var request = Api<ArtistChart>
				.Create
				.WithToDate(chartDate)
				.WithPeriod(ChartPeriod.Week)
				.WithPageSize(20);

			var artistChart = await request.Please();

			Assert.That(artistChart, Is.Not.Null);
			Assert.That(artistChart.ChartItems, Is.Not.Null);
			Assert.That(artistChart.ChartItems.Count, Is.EqualTo(20));

			Assert.That(artistChart.Type, Is.EqualTo(ChartType.artist));
			Assert.That(artistChart.FromDate, Is.LessThanOrEqualTo(chartDate));
			Assert.That(artistChart.ToDate, Is.GreaterThanOrEqualTo(chartDate));
			Assert.That(artistChart.ChartItems.FirstOrDefault().Artist, Is.Not.Null);
		}
	}
}