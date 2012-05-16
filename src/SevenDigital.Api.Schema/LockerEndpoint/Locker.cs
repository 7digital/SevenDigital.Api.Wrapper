using System.Collections.Generic;
using System.Xml.Serialization;
using SevenDigital.Api.Schema.OAuth;
using SevenDigital.Api.Schema.ParameterDefinitions.Get;
using SevenDigital.Api.Schema.Attributes;

namespace SevenDigital.Api.Schema.LockerEndpoint
{
	
	[ApiEndpoint("user/locker")]
	[XmlRoot("locker")]
	[OAuthSigned]
	public class Locker : HasPaging, HasReleaseIdParameter, HasTrackIdParameter, HasPurchaseIdParameter, HasLockerSort
	{
		[XmlArray("lockerReleases")]
		[XmlArrayItem("lockerRelease")]
		public List<LockerRelease> LockerReleases { get; set; }
	}
}