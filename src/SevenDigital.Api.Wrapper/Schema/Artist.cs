using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using SevenDigital.Api.Wrapper.Schema.Attributes;

namespace SevenDigital.Api.Wrapper.Schema
{
	[Serializable]
	[ApiEndpoint("artist/details")]
	[XmlRoot("artist")]
	public class Artist
	{
		[XmlAttribute("id")]
		public int Id { get; set; }

		[XmlElement("name")]
		public string Name { get; set; }

		[XmlElement("sortName")]
		public string SortName { get; set; }

		[XmlElement("appearsAs")]
		public string AppearsAs { get; set; }

		[XmlElement("image")]
		public string Image { get; set; }

		[XmlElement("url")]
		public string Url { get; set; }
	}

	[Serializable]
	[ApiEndpoint("artist/browse")]
	[XmlRoot("artists")]
	public class ArtistBrowse
	{
		[XmlElement("page")]
		public int Page { get; set; }

		[XmlElement("pageSize")]
		public int PageSize { get; set; }

		[XmlElement("totalItems")]
		public int TotalItems { get; set; }

		[XmlArray("artist")]
		public List<Artist> Artists { get; set; }
	}
}
