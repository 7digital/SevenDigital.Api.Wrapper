using System;
using System.Xml.Serialization;

namespace SevenDigital.Api.Wrapper.DTO
{
	[Serializable]
	[XmlRoot("serviceStatus")]
	public class Status
	{
		[XmlElement("serverTime")]
		public DateTime ServerTime { get; set; }
	}
}
