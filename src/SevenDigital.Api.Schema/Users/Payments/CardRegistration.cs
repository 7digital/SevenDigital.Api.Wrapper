using System;
using System.Xml.Serialization;
using SevenDigital.Api.Schema.Attributes;
using SevenDigital.Api.Schema.OAuth;

namespace SevenDigital.Api.Schema.Users.Payment
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
