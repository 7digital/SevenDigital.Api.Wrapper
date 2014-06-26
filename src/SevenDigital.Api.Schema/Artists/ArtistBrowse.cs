﻿using System.Collections.Generic;
using System.Xml.Serialization;
using SevenDigital.Api.Schema.Attributes;
using SevenDigital.Api.Schema.ParameterDefinitions.Get;

namespace SevenDigital.Api.Schema.Artists
{
	[ApiEndpoint("artist/browse")]
	[XmlRoot("artists")]
	public class ArtistBrowse : HasPaging, HasLetterParameter
	{
		[XmlElement("artist")]
		public List<Artist> Artists { get; set; }
	}
}