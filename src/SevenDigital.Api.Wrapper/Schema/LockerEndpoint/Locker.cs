using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using SevenDigital.Api.Wrapper.EndpointResolution.OAuth;
using SevenDigital.Api.Wrapper.Schema.Attributes;

namespace SevenDigital.Api.Wrapper.Schema.LockerEndpoint
{
    [Serializable]
    [ApiEndpoint("user/locker")]
    [XmlRoot("locker")]
	[OAuthSigned]
	public class Locker : HasPaging
    {
        [XmlArray("lockerReleases")]
        [XmlArrayItem("lockerRelease")]
        public List<LockerRelease> LockerReleases { get; set; }
    }
}