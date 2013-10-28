using SevenDigital.Api.Schema.Attributes;
using SevenDigital.Api.Schema.OAuth;
using System;
using System.Xml.Serialization;

namespace SevenDigital.Api.Schema.User.Payment
{
	[ApiEndpoint("user/payment/cardregistration")]
	[OAuthSigned]
	[HttpPost]
	[Serializable]
	[XmlRoot("cardRegistration")]
	public class CardRegistration
	{
		[XmlAttribute("id")]
		public string Id { get; set; }

		[XmlAttribute("status")]
		public CardRegistrationStatus Status { get; set; }

		[XmlElement("redirectUrl")]
		public string RedirectUrl { get; set; }
	}
}
