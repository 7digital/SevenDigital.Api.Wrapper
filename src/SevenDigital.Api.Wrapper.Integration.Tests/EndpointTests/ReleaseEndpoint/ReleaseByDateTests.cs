using System;
using NUnit.Framework;
using SevenDigital.Api.Schema.ReleaseEndpoint;

namespace SevenDigital.Api.Wrapper.Integration.Tests.EndpointTests.ReleaseEndpoint
{
	[TestFixture]
	[Category("Integration")]
	public class ReleaseByDateTests
	{
		[Test]
		public void Can_hit_endpoint()
		{
			ReleaseByDate release = Api<ReleaseByDate>.Get
				.WithParameter("toDate", DateTime.Now.ToString("yyyyMMdd"))
				.WithParameter("country", "GB")
				.Please();

			Assert.That(release, Is.Not.Null);
			Assert.That(release.Releases.Count, Is.GreaterThan(0));
		}

		[Test]
		public void Can_hit_endpoint_with_paging()
		{
			ReleaseByDate artistBrowse = Api<ReleaseByDate>.Get
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