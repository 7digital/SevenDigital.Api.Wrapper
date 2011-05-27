using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using SevenDigital.Api.Wrapper.EndpointResolution.OAuth;
using SevenDigital.Api.Schema.Attributes;
using SevenDigital.Api.Schema.LockerEndpoint;

namespace SevenDigital.Api.Schema.User.Purchase
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