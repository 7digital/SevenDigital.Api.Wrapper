using System;
using System.Xml.Serialization;
using SevenDigital.Api.Wrapper.EndpointResolution.OAuth;
using SevenDigital.Api.Wrapper.Schema.Attributes;

namespace SevenDigital.Api.Wrapper.Schema.User.Purchase
{
	[Serializable]
	[OAuthSigned]
	[ApiEndpoint("user/purchase/rrpitem")]
	[XmlRoot("purchase")]
	public class UserPurchaseRrpItem : BasePurchaseItem
	{}
}