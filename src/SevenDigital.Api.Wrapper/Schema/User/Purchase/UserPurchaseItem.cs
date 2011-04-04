using System;
using System.Xml.Serialization;
using SevenDigital.Api.Wrapper.EndpointResolution.OAuth;
using SevenDigital.Api.Wrapper.Schema.Attributes;

namespace SevenDigital.Api.Wrapper.Schema.User.Purchase
{
	[Serializable]
	[OAuthSigned]
	[ApiEndpoint("user/purchase/item")]
	[XmlRoot("purchase")]
	public class UserPurchaseItem : BasePurchaseItem
	{}
}