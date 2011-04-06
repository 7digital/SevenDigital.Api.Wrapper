using System.Xml.Serialization;
using SevenDigital.Api.Wrapper.Schema.Media;

namespace SevenDigital.Api.Wrapper.Schema.LockerEndpoint
{
	[XmlRoot("downloadUrl")]
	public class DownloadUrl
	{
		[XmlElement("url")]
		public string Url { get; set; }

		[XmlElement("format")]
		public Format Format { get; set; }
	}
}