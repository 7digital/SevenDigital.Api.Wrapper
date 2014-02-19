using System.Xml.Serialization;

namespace SevenDigital.Api.Schema.Playlists.Response
{
	[XmlRoot("link")]
	public class Link
	{
		public Link()
		{}

		public Link(string rel, string href)
		{
			Rel = rel;
			Href = href;
		}

		[XmlAttribute("rel")]
		public string Rel { get; set; }

		[XmlAttribute("href")]
		public string Href { get; set; }
	}
}