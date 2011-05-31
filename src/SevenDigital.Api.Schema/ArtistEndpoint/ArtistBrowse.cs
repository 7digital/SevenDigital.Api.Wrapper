using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using SevenDigital.Api.Schema.Attributes;

namespace SevenDigital.Api.Schema.ArtistEndpoint
{
	[ApiEndpoint("artist/browse")]
	[XmlRoot("artists")]
	public class ArtistBrowse : HasPaging
	{
		[XmlElement("artist")]
		public List<Artist> Artists { get; set; }
	}
}