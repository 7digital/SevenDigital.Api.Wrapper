using System.Xml.Serialization;
using SevenDigital.Api.Schema.Attributes;
using SevenDigital.Api.Schema.OAuth;

namespace SevenDigital.Api.Schema.Users.Purchase
{
	[OAuthSigned]
	[ApiEndpoint("user/purchase/item")]
	[XmlRoot("purchase")]
	public class UserPurchaseItem : BasePurchaseItem
	{
	}
}