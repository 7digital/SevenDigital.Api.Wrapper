using System.Collections.Generic;
using System.Xml.Serialization;
using SevenDigital.Api.Wrapper.Schema.ReleaseEndpoint;

namespace SevenDigital.Api.Wrapper.Schema.LockerEndpoint
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