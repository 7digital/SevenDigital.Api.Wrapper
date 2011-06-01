using System.Xml.Serialization;
using SevenDigital.Api.Schema.ReleaseEndpoint;

namespace SevenDigital.Api.Schema
{
	[XmlRoot("recommendedItem")]
	public class RecommendedItem
	{
		[XmlElement("release")]
		public Release Release { get; set; }
	}
}