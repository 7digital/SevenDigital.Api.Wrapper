using System.IO;
using System.Linq;
using System.Net;
using NUnit.Framework;
using SevenDigital.Api.Schema.ArtistEndpoint;
using SevenDigital.Api.Schema.ReleaseEndpoint;
using SevenDigital.Api.Wrapper.Responses;
using SevenDigital.Api.Wrapper.Responses.Parsing;

namespace SevenDigital.Api.Wrapper.Unit.Tests.Endpoints.Release
{
	[TestFixture]
	public class ReleaseEndpointTests
	{
		[Test]
		public void should_deserialize_emtpy_release_type_to_unknown()
		{
			var responseXml = File.ReadAllText("StubResponses/ArtistReleases.xml");
			var response = new Response(HttpStatusCode.OK, responseXml);

			var xmlParser = new ResponseParser(new ApiResponseDetector());
			var release = xmlParser.Parse<ArtistReleases>(response).Releases.First();

			Assert.That(release.Type,Is.EqualTo(ReleaseType.Unknown));
		}
	}
}
