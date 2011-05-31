using System;
using System.Xml.Serialization;
using SevenDigital.Api.Schema.Attributes;

namespace SevenDigital.Api.Schema.ArtistEndpoint
{
	[ApiEndpoint("artist/details")]
	[XmlRoot("artist")]
	public class Artist : IIsArtist
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

	public interface IIsArtist
	{
	}
}