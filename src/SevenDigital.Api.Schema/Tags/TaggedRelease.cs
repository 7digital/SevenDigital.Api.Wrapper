using System.Xml.Serialization;
using SevenDigital.Api.Schema.Releases;

namespace SevenDigital.Api.Schema.Tags
{
	[XmlRoot("taggedItem")]
	public class TaggedRelease
	{
		[XmlElement("release")]
		public Release Release { get; set; }
	}
}