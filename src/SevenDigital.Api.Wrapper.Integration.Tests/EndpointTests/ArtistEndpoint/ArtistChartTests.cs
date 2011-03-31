using System;
using System.Linq;
using NUnit.Framework;
using SevenDigital.Api.Wrapper.EndpointResolution;
using SevenDigital.Api.Wrapper.EndpointResolution.OAuth;
using SevenDigital.Api.Wrapper.Schema.ArtistEndpoint;
using SevenDigital.Api.Wrapper.Schema.Chart;
using SevenDigital.Api.Wrapper.Utility.Http;

namespace SevenDigital.Api.Wrapper.Integration.Tests.EndpointTests.ArtistEndpoint
{
	[TestFixture]
	[Category("Integration")]
	public class ArtistChartTests
	{
		[Test]
		public void Can_hit_endpoint()
		{
			var httpGetResolver = new EndpointResolver(new HttpGetResolver(), new UrlSigner(), new AppSettingsCredentials()); 

			ArtistChart artist = new FluentApi<ArtistChart>(httpGetResolver)
				.WithParameter("period", "week")
				.WithParameter("toDate", "20110131")
				.WithParameter("country", "GB")
				.Please();

			Assert.That(artist, Is.Not.Null);
			Assert.That(artist.ChartItems.Count, Is.EqualTo(10));
			Assert.That(artist.Type, Is.EqualTo(ChartType.artist));
			Assert.That(artist.FromDate, Is.EqualTo(new DateTime(2011, 01, 25)));
			Assert.That(artist.ToDate, Is.EqualTo(new DateTime(2011, 01, 31)));
			Assert.That(artist.ChartItems.FirstOrDefault().Artist, Is.Not.Null);
		}

		[Test]
		public void Can_hit_fluent_endpoint()
		{
			var artist = Api<ArtistChart>
							.Get
							.WithToDate(new DateTime(2011, 01, 31))
							.WithPeriod(ApiPeriod.Week)
							.Please();

			Assert.That(artist, Is.Not.Null);
			Assert.That(artist.ChartItems.Count, Is.EqualTo(10));
			Assert.That(artist.Type, Is.EqualTo(ChartType.artist));
			Assert.That(artist.FromDate, Is.EqualTo(new DateTime(2011, 01, 25)));
			Assert.That(artist.ToDate, Is.EqualTo(new DateTime(2011, 01, 31)));
			Assert.That(artist.ChartItems.FirstOrDefault().Artist, Is.Not.Null);
		}
	}
}