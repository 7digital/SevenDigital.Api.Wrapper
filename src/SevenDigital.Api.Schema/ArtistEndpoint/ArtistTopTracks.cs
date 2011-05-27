using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using SevenDigital.Api.Schema.Attributes;
using SevenDigital.Api.Schema.TrackEndpoint;

namespace SevenDigital.Api.Schema.ArtistEndpoint
{
	[Serializable]
	[ApiEndpoint("artist/toptracks")]
	[XmlRoot("tracks")]
	public class ArtistTopTracks : HasPaging, IIsArtist
	{
		[XmlElement("track")]
		public List<Track> Tracks { get; set; }
	}
}