using System.Threading;
using NUnit.Framework;
using SevenDigital.Api.Schema.ArtistEndpoint;
using SevenDigital.Api.Wrapper.Extensions.Get;

namespace SevenDigital.Api.Wrapper.Integration.Tests.EndpointTests.ArtistEndpoint
{
	[TestFixture]
	public class ArtistDetailsTests
	{
		[Test]
		public void Can_hit_endpoint_with_fluent_interface()
		{
			var artist = Api<ArtistIdParameter>
			    .Get
			    .WithArtistId(1)
			    .Please();

			Assert.That(artist, Is.Not.Null);
			Assert.That(artist.Name, Is.EqualTo("Keane"));
			Assert.That(artist.SortName, Is.EqualTo("Keane"));
			Assert.That(artist.Url, Is.EqualTo("http://www.7digital.com/artists/keane/?partner=1401"));
			Assert.That(artist.Image, Is.EqualTo("http://cdn.7static.com/static/img/artistimages/00/000/000/0000000001_150.jpg"));
		}

        [Test]
        public void Can_hit_endpoint_with_fluent_async_api()
        {
            ArtistIdParameter artistIdParameter = null;

            var reset = new AutoResetEvent(false);

               Api<ArtistIdParameter>
                .Get
                .WithArtistId(1)
                .PleaseAsync(payload =>
                                 {
                                     artistIdParameter = payload;
                                     reset.Set();
                                 });


            reset.WaitOne(1000 * 60);
            Assert.That(artistIdParameter, Is.Not.Null);
            Assert.That(artistIdParameter.Name, Is.EqualTo("Keane"));
            Assert.That(artistIdParameter.SortName, Is.EqualTo("Keane"));
            Assert.That(artistIdParameter.Url, Is.EqualTo("http://www.7digital.com/artists/keane/?partner=1401"));
            Assert.That(artistIdParameter.Image, Is.EqualTo("http://cdn.7static.com/static/img/artistimages/00/000/000/0000000001_150.jpg"));
        }
	}
}