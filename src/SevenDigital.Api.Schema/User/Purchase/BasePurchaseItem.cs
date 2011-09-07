using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using SevenDigital.Api.Schema.LockerEndpoint;
using SevenDigital.Api.Schema.ParameterDefinitions.Get;

namespace SevenDigital.Api.Schema.User.Purchase
{
	public abstract class BasePurchaseItem : HasReleaseIdParameter, HasPriceParameter, HasTrackIdParameter, HasBasketParameter
	{
		[XmlElement("purchaseDate")]
		public DateTime PurchaseDate { get; set; }

		[XmlArray("lockerReleases")]
		[XmlArrayItem("lockerRelease")]
		public List<LockerRelease> LockerReleases { get; set; }
	}
}