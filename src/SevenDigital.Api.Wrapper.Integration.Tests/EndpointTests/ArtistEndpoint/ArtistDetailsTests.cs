using NUnit.Framework;
using SevenDigital.Api.Wrapper.EndpointResolution;
using SevenDigital.Api.Wrapper.Schema.ArtistEndpoint;
using SevenDigital.Api.Wrapper.Utility.Http;

namespace SevenDigital.Api.Wrapper.Integration.Tests.EndpointTests.ArtistEndpoint
{
	[TestFixture]
	[Category("Integration")]
	public class ArtistDetailsTests
	{
		[Test]
		public void Can_hit_endpoint()
		{
			var httpGetResolver = new EndpointResolver(new HttpGetResolver()); // TODO: Set up using castle?

			Artist artist = new FluentApi<Artist>(httpGetResolver)
				.WithParameter("artistid","1")
				.Please();
			
			Assert.That(artist, Is.Not.Null);
			Assert.That(artist.Name, Is.EqualTo("Keane"));
			Assert.That(artist.SortName, Is.EqualTo("Keane"));
			Assert.That(artist.Url, Is.EqualTo("http://www.7digital.com/artists/keane/?partner=1401"));
			Assert.That(artist.Image, Is.EqualTo("http://cdn.7static.com/static/img/artistimages/00/000/000/0000000001_150.jpg"));
		}

		[Test]
		public void Can_hit_endpoint_with_fluent_interface()
		{
			var artist = (Artist)Api<Artist>
							.Get
							.WithArtistId(1)
							.Please();

			Assert.That(artist, Is.Not.Null);
			Assert.That(artist.Name, Is.EqualTo("Keane"));
			Assert.That(artist.SortName, Is.EqualTo("Keane"));
			Assert.That(artist.Url, Is.EqualTo("http://www.7digital.com/artists/keane/?partner=1401"));
			Assert.That(artist.Image, Is.EqualTo("http://cdn.7static.com/static/img/artistimages/00/000/000/0000000001_150.jpg"));
		}
	}
}