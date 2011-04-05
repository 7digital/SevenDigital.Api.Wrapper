using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using SevenDigital.Api.Wrapper.EndpointResolution.OAuth;
using SevenDigital.Api.Wrapper.Schema.Attributes;
using SevenDigital.Api.Wrapper.Schema.LockerEndpoint;

namespace SevenDigital.Api.Wrapper.Schema.User
{
	[Serializable]
	[OAuthSigned]
	[ApiEndpoint("user/deliveritem")]
	[XmlRoot("purchase")]
	public class UserDeliverItem
	{
		[XmlElement("purchaseDate")]
		public DateTime PurchaseDate { get; set; }

		[XmlArray("lockerReleases")]
		[XmlArrayItem("lockerRelease")]
		public List<LockerRelease> LockerReleases { get; set; }
	}
}