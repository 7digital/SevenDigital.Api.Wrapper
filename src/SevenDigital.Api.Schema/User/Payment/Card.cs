using System;
using System.Xml.Serialization;

namespace SevenDigital.Api.Schema.User.Payment
{
	[XmlRoot("card")]
	[Serializable]
	public class Card
	{
		[XmlAttribute("id")]
		public int Id { get; set; }
		[XmlElement("type")]
		public string Type { get; set; }

		[XmlElement("last4digits")]
		public string Last4Digits { get; set; }

		[XmlElement("cardHolderName")]
		public string CardHolderName { get; set; }

		[XmlElement("expiryDate")]
		public string ExpiryDate { get; set; }

		[XmlIgnore]
		public DateTime FormatedExpiryDate
		{
			get
			{
				return new DateTime(int.Parse(ExpiryDate.Substring(0, 4)),int.Parse(ExpiryDate.Substring(4)),1)
				.AddMonths(1)
				.AddMilliseconds(-1);
			}
		}

		[XmlElement("country")]
		public string IsoTwoLetterCountryCode { get; set; }
		
		[XmlElement("default")]
		public bool IsDefault { get; set; }
	}
}
