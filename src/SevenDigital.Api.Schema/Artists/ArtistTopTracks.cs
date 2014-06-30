using System.Collections.Generic;
using System.Xml.Serialization;
using SevenDigital.Api.Schema.Attributes;
using SevenDigital.Api.Schema.ParameterDefinitions.Get;
using SevenDigital.Api.Schema.Tracks;

namespace SevenDigital.Api.Schema.Artists
{
	[ApiEndpoint("artist/toptracks")]
	[XmlRoot("tracks")]
	public class ArtistTopTracks : HasPaging, HasArtistIdParameter
	{
		[XmlElement("track")]
		public List<Track> Tracks { get; set; }
	}
}