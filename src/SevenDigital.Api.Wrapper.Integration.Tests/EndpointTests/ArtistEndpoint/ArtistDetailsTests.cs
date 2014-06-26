using NUnit.Framework;
using SevenDigital.Api.Schema.Artists;

namespace SevenDigital.Api.Wrapper.Integration.Tests.EndpointTests.ArtistEndpoint
{
	[TestFixture]
	public class ArtistDetailsTests
	{
		[Test]
		public async void Can_hit_endpoint_with_fluent_interface()
		{
			var request = Api<Artist>
				.Create
				.WithArtistId(1);
			var artist = await request.Please();

			Assert.That(artist, Is.Not.Null);
			Assert.That(artist.Name, Is.EqualTo("Keane"));
			Assert.That(artist.SortName, Is.EqualTo("Keane"));
			Assert.That(artist.Url, Is.StringStarting("http://www.7digital.com/artist/keane/"));
			Assert.That(artist.Image, Is.EqualTo("http://cdn.7static.com/static/img/artistimages/00/000/000/0000000001_150.jpg"));
		}
	}
}