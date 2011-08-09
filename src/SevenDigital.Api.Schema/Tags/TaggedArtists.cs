using System.Xml.Serialization;
using SevenDigital.Api.Schema.ArtistEndpoint;
using SevenDigital.Api.Schema.ReleaseEndpoint;

namespace SevenDigital.Api.Schema.Tags
{
	[XmlRoot("taggedItem")]
	public class TaggedArtists
	{
		[XmlElement("artist")]
		public ArtistIdParameter ArtistIdParameter { get; set; }
	}

	[XmlRoot("taggedItem")]
	public class TaggedReleases
	{
		[XmlElement("release")]
		public Release Release { get; set; }
	}
}