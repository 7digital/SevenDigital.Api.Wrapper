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
			ReleaseChart release = await Api<ReleaseChart>.Create
				.WithParameter("fromDate", "20110101")
				.WithParameter("toDate", "20110301")
				.WithParameter("country", "GB")
				.Please();

			Assert.That(release, Is.Not.Null);
			Assert.That(release.ChartItems.Count, Is.GreaterThan(0));
			Assert.That(release.FromDate, Is.GreaterThan(new DateTime(2011, 01, 01)));
			Assert.That(release.ToDate, Is.EqualTo(new DateTime(2011, 03, 01)));
			Assert.That(release.Type, Is.EqualTo(ChartType.album));
		}

		[Test]
		public async void Can_hit_endpoint_with_paging()
		{
			ReleaseChart artistBrowse = await Api<ReleaseChart>.Create
				.WithParameter("fromDate", "20090610")
				.WithParameter("toDate", "20110101")
				.WithParameter("page", "2")
				.WithParameter("pageSize", "20")
				.Please();

			Assert.That(artistBrowse, Is.Not.Null);
			Assert.That(artistBrowse.Page, Is.EqualTo(2));
			Assert.That(artistBrowse.PageSize, Is.EqualTo(20));
		}

		[Test]
		public async void Can_hit_fluent_endpoint() 
		{
			var release = await Api<ReleaseChart>
							.Create
							.WithToDate(new DateTime(2011, 01, 31))
							.WithPeriod(ChartPeriod.Week)
							.Please();

			Assert.That(release, Is.Not.Null);
			Assert.That(release.ChartItems.Count, Is.EqualTo(10));
			Assert.That(release.Type, Is.EqualTo(ChartType.album));
			Assert.That(release.FromDate, Is.EqualTo(new DateTime(2011, 01, 25)));
			Assert.That(release.ToDate, Is.EqualTo(new DateTime(2011, 01, 31)));
			Assert.That(release.ChartItems.FirstOrDefault().Release, Is.Not.Null);
		}
	}
}