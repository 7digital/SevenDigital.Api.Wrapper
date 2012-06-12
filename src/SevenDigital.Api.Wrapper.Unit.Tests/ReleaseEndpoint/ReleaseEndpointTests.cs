using System.IO;
using System.Linq;
using System.Net;
using System.Xml.Linq;
using NUnit.Framework;
using SevenDigital.Api.Schema.ArtistEndpoint;
using SevenDigital.Api.Schema.ReleaseEndpoint;
using SevenDigital.Api.Wrapper.Utility.Http;
using SevenDigital.Api.Wrapper.Utility.Serialization;

namespace SevenDigital.Api.Wrapper.Unit.Tests.ReleaseEndpoint
{
	[TestFixture]
	public class ReleaseEndpointTests
	{
		[Test]
		public void should_deserialize_emtpy_release_type_to_unknown()
		{
			var responseXml = File.ReadAllText("StubResponses/ArtistReleases.xml");
			var response = new Response
				{
					StatusCode = HttpStatusCode.OK,
					Body = responseXml
				};

			var xmlSerializer = new ResponseDeserializer<ArtistReleases>();
			var release =  xmlSerializer.Deserialize(response).Releases.First();

			Assert.That(release.Type,Is.EqualTo(ReleaseType.Unknown));
		}
	}
}
