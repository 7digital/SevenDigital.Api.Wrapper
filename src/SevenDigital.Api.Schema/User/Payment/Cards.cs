using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using SevenDigital.Api.Schema.Attributes;
using SevenDigital.Api.Schema.OAuth;

namespace SevenDigital.Api.Schema.User.Payment
{
	[ApiEndpoint("user/payment/card")]
	[XmlRoot("cards")]
	[OAuthSigned]
	[RequireSecure]
	[Serializable]
	public class Cards
	{
		[XmlElement("card")]
		public List<Card> UserCards { get; set; }
	}
}
