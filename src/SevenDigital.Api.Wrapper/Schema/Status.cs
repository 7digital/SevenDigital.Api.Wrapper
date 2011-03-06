using System;
using System.Xml.Serialization;
using SevenDigital.Api.Wrapper.Schema.Attributes;

namespace SevenDigital.Api.Wrapper.Schema
{
	[Serializable]
	[ApiEndpoint("status")]
	[XmlRoot("serviceStatus")]
	public class Status
	{
		[XmlElement("serverTime")]
		public DateTime ServerTime { get; set; }
	}
}
