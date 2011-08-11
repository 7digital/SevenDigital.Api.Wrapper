using System.Xml.Serialization;

namespace SevenDigital.Api.Schema.ReleaseEndpoint
{
	
	public enum ReleaseType
	{
		Single,
		Album,
		Video,
		Exclusive,
		Item,
		[XmlEnum(Name = "")]
		Unknown,
	}
}