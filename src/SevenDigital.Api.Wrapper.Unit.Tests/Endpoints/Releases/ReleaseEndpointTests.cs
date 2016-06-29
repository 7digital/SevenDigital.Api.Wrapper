using System.IO;
using System.Linq;
using System.Net;
using NUnit.Framework;
using SevenDigital.Api.Schema.Artists;
using SevenDigital.Api.Schema.Releases;
using SevenDigital.Api.Wrapper.Responses;
using SevenDigital.Api.Wrapper.Responses.Parsing;

namespace SevenDigital.Api.Wrapper.Unit.Tests.Endpoints.Releases
{
	[TestFixture]
	public class ReleaseEndpointTests
	{
		[Test]
		public void should_deserialize_empty_release_type_to_unknown()
		{
			var responseXml = File.ReadAllText("StubResponses/ArtistReleases.xml");
			var response = ResponseCreator.FromBody(HttpStatusCode.OK, responseXml);

			var xmlParser = new ResponseParser(new ApiResponseDetector());
			var release = xmlParser.Parse<ArtistReleases>(response).Releases.First();

			Assert.That(release.Type,Is.EqualTo(ReleaseType.Unknown));
		}
	}
}
