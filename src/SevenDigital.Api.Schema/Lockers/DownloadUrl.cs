using System;
using System.Xml.Serialization;
using SevenDigital.Api.Schema.Media;

namespace SevenDigital.Api.Schema.Lockers
{
	[Serializable]
	[XmlRoot("downloadUrl")]
	public class DownloadUrl
	{
		[XmlElement("url")]
		public string Url { get; set; }

		[XmlElement("format")]
		public Format Format { get; set; }
	}
}