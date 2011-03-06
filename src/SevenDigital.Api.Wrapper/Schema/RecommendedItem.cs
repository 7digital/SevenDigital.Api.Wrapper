using System.Xml.Serialization;

namespace SevenDigital.Api.Wrapper.Schema
{
	[XmlRoot("recommendedItem")]
	public class RecommendedItem
	{
		[XmlElement("release")]
		public Release Release { get; set; }
	}
}