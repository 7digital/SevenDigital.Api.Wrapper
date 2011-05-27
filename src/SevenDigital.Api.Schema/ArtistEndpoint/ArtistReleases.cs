using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using SevenDigital.Api.Schema.Attributes;
using SevenDigital.Api.Schema.ReleaseEndpoint;

namespace SevenDigital.Api.Schema.ArtistEndpoint
{
	[Serializable]
	[ApiEndpoint("artist/releases")]
	[XmlRoot("releases")]
	public class ArtistReleases : HasPaging, IIsArtist
	{
		[XmlElement("release")]
		public List<Release> Releases { get; set; }
	}
}