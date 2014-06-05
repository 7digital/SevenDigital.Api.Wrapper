using System;
using System.Linq;
using NUnit.Framework;
using SevenDigital.Api.Schema.Chart;
using SevenDigital.Api.Schema.ReleaseEndpoint;

namespace SevenDigital.Api.Wrapper.Integration.Tests.EndpointTests.ReleaseEndpoint
{
	[TestFixture]
	[Category("Integration")]
	public class ReleaseChartTests
	{
		[Test]
		public async void Can_hit_endpoint()
		{
			var request = Api<ReleaseChart>.Create
				.WithParameter("fromDate", "20110101")
				.WithParameter("toDate", "20110301")
				.WithParameter("country", "GB");
			var releaseChart = await request.Please();

			Assert.That(releaseChart, Is.Not.Null);
			Assert.That(releaseChart.ChartItems.Count, Is.GreaterThan(0));
			Assert.That(releaseChart.FromDate, Is.GreaterThan(new DateTime(2011, 01, 01)));
			Assert.That(releaseChart.ToDate, Is.EqualTo(new DateTime(2011, 03, 01)));
			Assert.That(releaseChart.Type, Is.EqualTo(ChartType.album));
		}

		[Test]
		public async void Can_hit_endpoint_with_paging()
		{
			var request = Api<ReleaseChart>.Create
				.WithParameter("fromDate", "20090610")
				.WithParameter("toDate", "20110101")
				.WithParameter("page", "2")
				.WithParameter("pageSize", "20");
			var releaseChart = await request.Please();

			Assert.That(releaseChart, Is.Not.Null);
			Assert.That(releaseChart.Page, Is.EqualTo(2));
			Assert.That(releaseChart.PageSize, Is.EqualTo(20));
		}

		[Test]
		public async void Can_hit_fluent_endpoint()
		{
			var request = Api<ReleaseChart>
				.Create
				.WithToDate(new DateTime(2011, 01, 31))
				.WithPeriod(ChartPeriod.Week);
			var releaseChart = await request.Please();

			Assert.That(releaseChart, Is.Not.Null);
			Assert.That(releaseChart.ChartItems.Count, Is.EqualTo(10));
			Assert.That(releaseChart.Type, Is.EqualTo(ChartType.album));
			Assert.That(releaseChart.FromDate, Is.EqualTo(new DateTime(2011, 01, 25)));
			Assert.That(releaseChart.ToDate, Is.EqualTo(new DateTime(2011, 01, 31)));
			Assert.That(releaseChart.ChartItems.FirstOrDefault().Release, Is.Not.Null);
		}
	}
}