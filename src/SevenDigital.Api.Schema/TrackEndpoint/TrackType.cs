using System.Xml.Serialization;

namespace SevenDigital.Api.Schema.TrackEndpoint
{
	public enum TrackType
	{
		[XmlEnum(Name = "audio")]
		Audio,
		[XmlEnum(Name = "video")]
		Video,
		[XmlEnum(Name = "pdf")]
		Pdf,
		[XmlEnum(Name = "zip")]
		Zip,
		[XmlEnum(Name = "")]
		Unknown,
	}
}