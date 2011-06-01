using System;
using System.Xml.Serialization;
using SevenDigital.Api.Wrapper.EndpointResolution.OAuth;
using SevenDigital.Api.Schema.Attributes;

namespace SevenDigital.Api.Schema.User.Purchase
{
	
	[OAuthSigned]
	[ApiEndpoint("user/purchase/rrpitem")]
	[XmlRoot("purchase")]
	public class UserPurchaseRrpItem : BasePurchaseItem
	{
	}
}