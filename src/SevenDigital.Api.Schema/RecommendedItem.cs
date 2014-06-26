using System;
using System.Xml.Serialization;
using SevenDigital.Api.Schema.Releases;

namespace SevenDigital.Api.Schema
{
	[XmlRoot("recommendedItem")]
	[Serializable]
	public class RecommendedItem
	{
		[XmlElement("release")]
		public Release Release { get; set; }
	}
}