using System.Net;
using NUnit.Framework;
using SevenDigital.Api.Schema.User.Payment;
using SevenDigital.Api.Wrapper.Responses;
using SevenDigital.Api.Wrapper.Responses.Parsing;

namespace SevenDigital.Api.Wrapper.Unit.Tests.Parsing.User.Payment
{
	[TestFixture]
	public class DeleteCardTests
	{
		[Test]
		public void Can_Deserialise_ok_response_without_body__as_DeleteCard()
		{
			const string ResponseXml = "<?xml version=\"1.0\" encoding=\"iso-8859-1\"?><response status=\"ok\" version=\"1.2\" />";

			var response = new Response(HttpStatusCode.OK, ResponseXml);

			var xmlParser = new ResponseParser<DeleteCard>();
			var result = xmlParser.Parse(response);

			Assert.That(result, Is.Not.Null);
		}
	}
}
