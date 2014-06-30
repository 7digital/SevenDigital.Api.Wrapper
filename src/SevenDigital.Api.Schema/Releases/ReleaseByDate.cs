﻿using System.Collections.Generic;
using System.Xml.Serialization;
using SevenDigital.Api.Schema.Attributes;

namespace SevenDigital.Api.Schema.Releases
{
	[ApiEndpoint("release/bydate")]
	[XmlRoot("releases")]
	public class ReleaseByDate : HasPaging
	{
		[XmlElement("release")]
		public List<Release> Releases { get; set; }
	}
}