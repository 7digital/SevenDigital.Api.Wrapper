using System.Xml.Serialization;
using SevenDigital.Api.Schema.Attributes;
using SevenDigital.Api.Schema.OAuth;
using SevenDigital.Api.Schema.ParameterDefinitions.Get;
using SevenDigital.Api.Schema.Pricing;

namespace SevenDigital.Api.Schema.Baskets
{
	[ApiEndpoint("basket")]
	[XmlRoot("basket")]
	public class Basket : HasBasketParameter
	{
		[XmlAttribute("id")]
		public string Id { get; set; }

		[XmlElement("price")]
		public Price Price { get; set; }

		[XmlElement("basketItems")]
		public BasketItemList BasketItems { get; set; }

		[XmlElement("amountDue")]
		public AmountDue AmountDue { get; set; }
	}

	[ApiEndpoint("basket/addItem")]
	[XmlRoot("basket")]
	public class AddItemToBasket : Basket, HasReleaseIdParameter, HasTrackIdParameter, HasAffiliatePartnerParameter
	{}

	[ApiEndpoint("basket/create")]
	[XmlRoot("basket")]
	public class CreateBasket : Basket, HasAffiliatePartnerParameter
	{}

	[ApiEndpoint("basket/removeItem")]
	[XmlRoot("basket")]
	public class RemoveItemFromBasket : Basket, HasBasketItemParameter
	{}

	[HttpPost]
	[RequireSecure]
	[OAuthSigned]
	[ApiEndpoint("basket/applyvoucher")]
	[XmlRoot("basket")]
	public class ApplyVoucher : Basket
	{
	}
}