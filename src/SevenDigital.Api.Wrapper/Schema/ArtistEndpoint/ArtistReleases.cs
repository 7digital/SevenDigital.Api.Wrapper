using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using SevenDigital.Api.Wrapper.Schema.Attributes;
using SevenDigital.Api.Wrapper.Schema.ReleaseEndpoint;

namespace SevenDigital.Api.Wrapper.Schema.ArtistEndpoint
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