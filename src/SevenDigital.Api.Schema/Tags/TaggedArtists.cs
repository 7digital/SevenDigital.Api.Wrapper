using System.Xml.Serialization;
using SevenDigital.Api.Schema.ArtistEndpoint;

namespace SevenDigital.Api.Schema.Tags
{
	[XmlRoot("taggedItem")]
	public class TaggedArtists
	{
		[XmlElement("artist")]
		public Artist Artist { get; set; }
	}
}