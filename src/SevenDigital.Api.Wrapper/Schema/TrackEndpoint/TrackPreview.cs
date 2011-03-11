using System;
using System.Xml.Serialization;
using SevenDigital.Api.Wrapper.Schema.Attributes;

namespace SevenDigital.Api.Wrapper.Schema.TrackEndpoint
{
	[Serializable]
	[ApiEndpoint("track/preview")]
	[XmlRoot("url")]
	public class TrackPreview
	{
		[XmlText]
		public string Url { get; set; }
	}
}