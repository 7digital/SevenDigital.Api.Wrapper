using System.Collections.Generic;
using System.Xml.Serialization;
using SevenDigital.Api.Schema.ReleaseEndpoint;

namespace SevenDigital.Api.Schema.LockerEndpoint
{
	[XmlRoot("lockerRelease")]
	public class LockerRelease
	{
		[XmlElement("release")]
		public Release Release { get; set; }

		[XmlArray("lockerTracks")]
		[XmlArrayItem("lockerTrack")]
		public List<LockerTrack> LockerTracks { get; set; }
	}
}