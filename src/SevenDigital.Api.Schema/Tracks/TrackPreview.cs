using System.Xml.Serialization;
using SevenDigital.Api.Schema.Attributes;

namespace SevenDigital.Api.Schema.Tracks
{
	[ApiEndpoint("track/preview")]
	[XmlRoot("url")]
	public class TrackPreview
	{
		[XmlText]
		public string Url { get; set; }
	}
}