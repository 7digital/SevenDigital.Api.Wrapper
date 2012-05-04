using System.Linq;
using NUnit.Framework;
using SevenDigital.Api.Schema.Payment;
using SevenDigital.Api.Wrapper.Utility.Serialization;

namespace SevenDigital.Api.Wrapper.Unit.Tests.Deserialisation.Payment
{
	[TestFixture]
	public class CardTypeTests
	{
		private const string RESPONSE = " <response status=\"ok\" version=\"1.2\"><cardTypes><cardType id=\"MAESTRO\">Maestro</cardType><cardType id=\"MASTERCARD\">MasterCard</cardType><cardType id=\"VISA\">Visa</cardType><cardType id=\"AMEX\">American Express</cardType></cardTypes></response>";

		[Test]
		public void can_deseralize_card_types()
		{
			var xmlSerializer = new ApiXmlDeSerializer<PaymentCardTypes>(new ApiResourceDeSerializer<PaymentCardTypes>(), new XmlErrorHandler());

			var result = xmlSerializer.DeSerialize(RESPONSE);

			Assert.That(result.CardTypes.Count(),Is.EqualTo(4));
			var lastCard = result.CardTypes.Last();
			Assert.That(lastCard.Type, Is.EqualTo("American Express"));
			Assert.That(lastCard.Id, Is.EqualTo("AMEX"));
		}
	}
}
