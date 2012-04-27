using System;
using System.Xml.Serialization;
using SevenDigital.Api.Schema.Attributes;
using SevenDigital.Api.Schema.OAuth;

namespace SevenDigital.Api.Schema.User.Purchase
{
	
	[OAuthSigned]
	[ApiEndpoint("user/purchase/rrpitem")]
	[XmlRoot("purchase")]
	public class UserPurchaseRrpItem : BasePurchaseItem
	{
	}
}