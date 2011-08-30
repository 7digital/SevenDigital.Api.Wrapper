using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace SevenDigital.Api.Schema.ReleaseEndpoint
{
	
	[XmlRoot("searchResult")]
	public class ReleaseSearchResult
	{
		[XmlElement("type")]
		public ItemType Type { get; set; }

		[XmlElement("release")]
		public Release Release { get; set; }
	}
}