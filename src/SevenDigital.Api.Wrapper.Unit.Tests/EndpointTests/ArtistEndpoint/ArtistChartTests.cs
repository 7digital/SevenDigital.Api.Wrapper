using System;
using System.Linq;
using NUnit.Framework;
using SevenDigital.Api.Wrapper.EndpointResolution;
using SevenDigital.Api.Wrapper.Repository;
using SevenDigital.Api.Wrapper.Schema;
using SevenDigital.Api.Wrapper.Utility.Http;

namespace SevenDigital.Api.Wrapper.Unit.Tests.EndpointTests.ArtistEndpoint
{
	[TestFixture]
	[Category("Integration")]
	public class ArtistChartTests
	{
		[Test]
		public void Can_hit_endpoint()
		{
			var httpGetResolver = new EndpointResolver(new HttpGetResolver()); // TODO: Set up using castle?

			ArtistChart artist = new FluentApi<ArtistChart>(httpGetResolver)
				.WithParameter("period", "week")
				.WithParameter("toDate", "20110131")
				.WithParameter("country", "GB")
				.Resolve();

			Assert.That(artist, Is.Not.Null);
			Assert.That(artist.ChartItems.Count, Is.EqualTo(10));
			Assert.That(artist.Type, Is.EqualTo(ChartType.artist));
			Assert.That(artist.FromDate, Is.EqualTo(new DateTime(2011, 01, 25)));
			Assert.That(artist.ToDate, Is.EqualTo(new DateTime(2011, 01, 31)));
			Assert.That(artist.ChartItems.FirstOrDefault().Artist, Is.Not.Null);
		}
	}
}