using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using SevenDigital.Api.Wrapper.EndpointResolution.OAuth;
using SevenDigital.Api.Wrapper.Schema.Attributes;
using SevenDigital.Api.Wrapper.Schema.LockerEndpoint;

namespace SevenDigital.Api.Wrapper.Schema.User.Purchase
{
	[Serializable]
	[OAuthSigned]
	[ApiEndpoint("user/purchase/basket")]
	[XmlRoot("purchase")]
	public class UserPurchaseBasket
	{
		[XmlElement("purchaseDate")]
		public DateTime PurchaseDate { get; set; }

		[XmlArray("lockerReleases")]
		[XmlArrayItem("lockerRelease")]
		public List<LockerRelease> LockerReleases { get; set; }
	}
}