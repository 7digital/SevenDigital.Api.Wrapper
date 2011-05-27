using NUnit.Framework;
using SevenDigital.Api.Schema.ArtistEndpoint;
using SevenDigital.Api.Wrapper.Schema.ArtistEndpoint;

namespace SevenDigital.Api.Wrapper.Integration.Tests.EndpointTests.ArtistEndpoint
{
	[TestFixture]
	public class ArtistDetailsTests
	{
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