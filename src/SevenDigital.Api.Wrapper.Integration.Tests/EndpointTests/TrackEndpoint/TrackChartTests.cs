using System;
using NUnit.Framework;
using SevenDigital.Api.Schema.Chart;
using SevenDigital.Api.Schema.TrackEndpoint;

namespace SevenDigital.Api.Wrapper.Integration.Tests.EndpointTests.TrackEndpoint
{
	[TestFixture]
	public class TrackChartTests
	{
		[Test]
		public void Can_hit_endpoint()
		{

			TrackChart release = Api<TrackChart>.Get
				.WithParameter("fromDate", "20110101")
				.WithParameter("toDate", "20110301")
				.WithParameter("country", "GB")
				.Please();

			Assert.That(release, Is.Not.Null);
			Assert.That(release.ChartItems.Count, Is.GreaterThan(0));
			Assert.That(release.FromDate, Is.GreaterThan(new DateTime(2011, 01, 01)));
			Assert.That(release.ToDate, Is.EqualTo(new DateTime(2011, 03, 01)));
			Assert.That(release.Type, Is.EqualTo(ChartType.track));

		}

		[Test]
		public void Can_hit_endpoint_with_paging()
		{
			TrackChart artistBrowse = Api<TrackChart>.Get
				.WithParameter("fromDate", "20090610")
				.WithParameter("toDate", "20110101")
				.WithParameter("page", "2")
				.WithParameter("pageSize", "20")
				.Please();

			Assert.That(artistBrowse, Is.Not.Null);
			Assert.That(artistBrowse.Page, Is.EqualTo(2));
			Assert.That(artistBrowse.PageSize, Is.EqualTo(20));
		}
	}
}