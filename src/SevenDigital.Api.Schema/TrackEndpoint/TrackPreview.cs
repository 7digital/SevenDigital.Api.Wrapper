using System;
using System.Xml.Serialization;
using SevenDigital.Api.Schema.Attributes;

namespace SevenDigital.Api.Schema.TrackEndpoint
{
	
	[ApiEndpoint("track/preview")]
	[XmlRoot("url")]
	public class TrackPreview
	{
		[XmlText]
		public string Url { get; set; }
	}
}