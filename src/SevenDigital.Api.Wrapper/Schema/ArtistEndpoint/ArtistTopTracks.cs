using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using SevenDigital.Api.Wrapper.Schema.Attributes;
using SevenDigital.Api.Wrapper.Schema.TrackEndpoint;

namespace SevenDigital.Api.Wrapper.Schema.ArtistEndpoint
{
	[Serializable]
	[ApiEndpoint("artist/toptracks")]
	[XmlRoot("tracks")]
	public class ArtistTopTracks : HasPaging
	{
		[XmlElement("track")]
		public List<Track> Tracks { get; set; }
	}
}