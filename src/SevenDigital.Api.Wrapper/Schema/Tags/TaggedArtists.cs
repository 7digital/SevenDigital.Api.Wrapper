using System.Xml.Serialization;
using SevenDigital.Api.Wrapper.Schema.ArtistEndpoint;
using SevenDigital.Api.Wrapper.Schema.ReleaseEndpoint;

namespace SevenDigital.Api.Wrapper.Schema.Tags
{
	[XmlRoot("taggedItem")]
	public class TaggedArtists
	{
		[XmlElement("artist")]
		public Artist Artist { get; set; }
	}

	[XmlRoot("taggedItem")]
	public class TaggedReleases
	{
		[XmlElement("release")]
		public Release Release { get; set; }
	}
}