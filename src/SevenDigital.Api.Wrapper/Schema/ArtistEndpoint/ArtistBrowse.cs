using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using SevenDigital.Api.Wrapper.Schema.Attributes;

namespace SevenDigital.Api.Wrapper.Schema.ArtistEndpoint
{
	[Serializable]
	[ApiEndpoint("artist/browse")]
	[XmlRoot("artists")]
	public class ArtistBrowse : HasPaging
	{
		[XmlElement("artist")]
		public List<Artist> Artists { get; set; }
	}
}