using System.IO;
using System.Linq;
using System.Xml.Linq;
using NUnit.Framework;
using SevenDigital.Api.Schema.ArtistEndpoint;
using SevenDigital.Api.Schema.ReleaseEndpoint;
using SevenDigital.Api.Wrapper.Utility.Serialization;

namespace SevenDigital.Api.Wrapper.Unit.Tests.ReleaseEndpoint
{
	[TestFixture]
	public class ReleaseEndpointTests
	{
		[Test]
		public void should_deserialize_emtpy_release_type_to_unknown()
		{
			var response = XDocument.Load("StubResponses/ArtistReleases.xml");
			var xmlSerializer = new ApiXmlDeSerializer<ArtistReleases>(new ApiResourceDeSerializer<ArtistReleases>());
			var release =  xmlSerializer.DeSerialize(response.ToString()).Releases.First();

			Assert.That(release.Type,Is.EqualTo(ReleaseType.Unknown));
		}
	}
}
