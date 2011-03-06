using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace SevenDigital.Api.Wrapper.Schema.ReleaseEndpoint
{
	[Serializable]
	[XmlRoot("searchResult")]
	public class ReleaseSearchResult
	{
		[XmlElement("type")]
		public ItemType Type { get; set; }

		[XmlElement("release")]
		public List<Release> Releases { get; set; }
	}
}