using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using SevenDigital.Api.Schema.Attributes;
using SevenDigital.Api.Schema.LockerEndpoint;
using SevenDigital.Api.Schema.OAuth;

namespace SevenDigital.Api.Schema.User.Purchase
{
	
	[OAuthSigned]
	[ApiEndpoint("user/purchase/basket")]
	[XmlRoot("purchase")]
	public class UserPurchaseBasket
	{
		[XmlAttribute("id")]
		public string Id { get; set; }

		[XmlElement("purchaseDate")]
		public DateTime PurchaseDate { get; set; }

		[XmlArray("lockerReleases")]
		[XmlArrayItem("lockerRelease")]
		public List<LockerRelease> LockerReleases { get; set; }
	}
}