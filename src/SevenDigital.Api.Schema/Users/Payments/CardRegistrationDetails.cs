using System;
using System.Xml.Serialization;
using SevenDigital.Api.Schema.Attributes;
using SevenDigital.Api.Schema.OAuth;

namespace SevenDigital.Api.Schema.Users.Payment
{
	[ApiEndpoint("user/payment/cardregistration/{id}")]
	[OAuthSigned]
	[Serializable]
	[XmlRoot("cardRegistration")]
	public class CardRegistrationDetails
	{
		[XmlAttribute("id")]
		public string Id { get; set; }

		[XmlAttribute("status")]
		public CardRegistrationStatus Status { get; set; }

		[XmlElement("redirectUrl")]
		public string RedirectUrl { get; set; }

		[XmlElement("card")]
		public Card Card { get; set; }

		[XmlElement("error")]
		public Error Error { get; set; }
	}
}
