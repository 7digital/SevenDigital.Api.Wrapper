using System.Xml.Serialization;
using SevenDigital.Api.Wrapper.Schema.ReleaseEndpoint;

namespace SevenDigital.Api.Wrapper.Schema
{
	[XmlRoot("recommendedItem")]
	public class RecommendedItem
	{
		[XmlElement("release")]
		public Release Release { get; set; }
	}
}