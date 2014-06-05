using System;
using System.Linq;
using System.Net;
using NUnit.Framework;
using SevenDigital.Api.Schema.User.Payment;
using SevenDigital.Api.Wrapper.Responses;
using SevenDigital.Api.Wrapper.Responses.Parsing;

namespace SevenDigital.Api.Wrapper.Unit.Tests.Parsing.User.Payment
{
	[TestFixture]
	public class Cards_unit_tests
	{
		private const string responseXml = 
			"<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
				"<response xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:noNamespaceSchemaLocation=\"http://api.7digital.com/1.2/static/7digitalAPI.xsd\" status=\"ok\" version=\"1.2\">" +
				"<cards>" +
					"<card id=\"8154430\">" +
					"<type>Visa</type>" +
					"<last4digits>5485</last4digits>" +
					"<default>true</default>" +
					"<cardHolderName>MR I HUNT</cardHolderName>" +
					"<expiryDate>201202</expiryDate>" +
					"<country>NZ</country>" +
				"</card>" +
				"</cards>" +
				"</response>";

		private readonly Response response = new Response( HttpStatusCode.OK, responseXml);

		[Test]
		public void can_deserialise_response_user_cards()
		{
			var xmlParser = new ResponseParser<Cards>(new ApiResponseDetector());

			var deserializedCards = xmlParser.Parse(response);

			var firstCard = deserializedCards.UserCards[0];
			Assert.That(deserializedCards.UserCards.Count(), Is.EqualTo(1));

			Assert.That(firstCard.Id, Is.EqualTo(8154430));
			Assert.That(firstCard.CardHolderName, Is.EqualTo("MR I HUNT"));
			Assert.That(firstCard.Type, Is.EqualTo("Visa"));
			Assert.That(firstCard.Last4Digits, Is.EqualTo("5485"));
			Assert.That(firstCard.IsoTwoLetterCountryCode, Is.EqualTo("NZ"));


			Assert.That(firstCard.ExpiryDate, Is.EqualTo("201202"));
			Assert.That(firstCard.FormatedExpiryDate, Is.EqualTo(new DateTime(2012, 02, 29, 23, 59, 59, 999)));
		}
	}
}
