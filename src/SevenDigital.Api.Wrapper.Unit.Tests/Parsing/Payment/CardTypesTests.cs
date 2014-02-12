using System.Linq;
using System.Net;
using NUnit.Framework;
using SevenDigital.Api.Schema.Payment;
using SevenDigital.Api.Wrapper.Responses;
using SevenDigital.Api.Wrapper.Responses.Parsing;

namespace SevenDigital.Api.Wrapper.Unit.Tests.Parsing.Payment
{
	[TestFixture]
	public class CardTypeTests
	{
		private const string ResponseBody = "<?xml version=\"1.0\" encoding=\"utf-8\"?><response status=\"ok\" version=\"1.2\"><cardTypes><cardType id=\"MAESTRO\">Maestro</cardType><cardType id=\"MASTERCARD\">MasterCard</cardType><cardType id=\"VISA\">Visa</cardType><cardType id=\"AMEX\">American Express</cardType></cardTypes></response>";

		private readonly Response stubResponse = new Response(HttpStatusCode.OK, ResponseBody);

		[Test]
		public void can_deseralize_card_types()
		{
			var xmlParser = new ResponseParser<PaymentCardTypes>();

			var result = xmlParser.Parse(stubResponse);

			Assert.That(result.CardTypes.Count(),Is.EqualTo(4));
			var lastCard = result.CardTypes.Last();
			Assert.That(lastCard.Type, Is.EqualTo("American Express"));
			Assert.That(lastCard.Id, Is.EqualTo("AMEX"));
		}
	}
}
