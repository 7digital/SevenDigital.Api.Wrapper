using System;
using System.Linq;
using NUnit.Framework;
using SevenDigital.Api.Schema.Charts;
using SevenDigital.Api.Schema.Tracks;

namespace SevenDigital.Api.Wrapper.Integration.Tests.EndpointTests.Tracks
{
	[TestFixture]
	public class TrackChartTests
	{
		[Test]
		public async void Can_hit_endpoint()
		{
			var request = Api<TrackChart>.Create
				.WithParameter("fromDate", "20110101")
				.WithParameter("toDate", "20110301")
				.WithParameter("country", "GB");
			var trackChart = await request.Please();

			Assert.That(trackChart, Is.Not.Null);
			Assert.That(trackChart.ChartItems.Count, Is.GreaterThan(0));
			Assert.That(trackChart.FromDate, Is.GreaterThan(new DateTime(2011, 01, 01)));
			Assert.That(trackChart.ToDate, Is.EqualTo(new DateTime(2011, 03, 01)));
			Assert.That(trackChart.Type, Is.EqualTo(ChartType.track));

		}

		[Test]
		public async void Can_hit_endpoint_with_paging()
		{
			var request = Api<TrackChart>.Create
				.WithParameter("fromDate", "20090610")
				.WithParameter("toDate", "20110101")
				.WithParameter("page", "2")
				.WithParameter("pageSize", "20");
			TrackChart trackChart = await request.Please();

			Assert.That(trackChart, Is.Not.Null);
			Assert.That(trackChart.Page, Is.EqualTo(2));
			Assert.That(trackChart.PageSize, Is.EqualTo(20));
		}

		[Test]
		public async void Can_hit_fluent_endpoint()
		{
			var request = Api<TrackChart>
				.Create
				.WithToDate(new DateTime(2011, 01, 31))
				.WithPeriod(ChartPeriod.Week);
			var trackChart = await request.Please();

			Assert.That(trackChart, Is.Not.Null);
			Assert.That(trackChart.ChartItems, Is.Not.Null);
			Assert.That(trackChart.ChartItems.Count, Is.EqualTo(10));
			Assert.That(trackChart.Type, Is.EqualTo(ChartType.track));
			Assert.That(trackChart.FromDate, Is.EqualTo(new DateTime(2011, 01, 25)));
			Assert.That(trackChart.ToDate, Is.EqualTo(new DateTime(2011, 01, 31)));
			Assert.That(trackChart.ChartItems.FirstOrDefault().Track, Is.Not.Null);
		}
	}
}