using System.IO;
using System.Linq;
using System.Net;
using NUnit.Framework;
using SevenDigital.Api.Schema.ArtistEndpoint;
using SevenDigital.Api.Schema.ReleaseEndpoint;
using SevenDigital.Api.Wrapper.Http;
using SevenDigital.Api.Wrapper.Serialization;

namespace SevenDigital.Api.Wrapper.Unit.Tests.ReleaseEndpoint
{
	[TestFixture]
	public class ReleaseEndpointTests
	{
		[Test]
		public void should_deserialize_emtpy_release_type_to_unknown()
		{
			var responseXml = File.ReadAllText("StubResponses/ArtistReleases.xml");
			var response = new Response(HttpStatusCode.OK, responseXml);

			var xmlParser = new ResponseParser<ArtistReleases>();
            var release = xmlParser.Parse(response, false).Releases.First();

			Assert.That(release.Type,Is.EqualTo(ReleaseType.Unknown));
		}
	}
}
