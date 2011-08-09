using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using SevenDigital.Api.Schema.ParameterDefinitions.Get;
using SevenDigital.Api.Schema.User.Purchase;
using SevenDigital.Api.Wrapper.EndpointResolution.OAuth;
using SevenDigital.Api.Schema.Attributes;

namespace SevenDigital.Api.Schema.LockerEndpoint
{
	
	[ApiEndpoint("user/locker")]
	[XmlRoot("locker")]
	[OAuthSigned]
	public class Locker : HasPaging, HasReleaseIdParameter, HasTrackIdParameter
	{
		[XmlArray("lockerReleases")]
		[XmlArrayItem("lockerRelease")]
		public List<LockerRelease> LockerReleases { get; set; }
	}
}