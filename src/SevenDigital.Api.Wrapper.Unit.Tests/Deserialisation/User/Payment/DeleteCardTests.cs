using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using NUnit.Framework;
using SevenDigital.Api.Schema.Payment;
using SevenDigital.Api.Schema.User.Payment;
using SevenDigital.Api.Wrapper.Utility.Http;
using SevenDigital.Api.Wrapper.Utility.Serialization;

namespace SevenDigital.Api.Wrapper.Unit.Tests.Deserialisation.User.Payment
{
	[TestFixture]
	public class DeleteCardTests
	{
		[Test]
		public void Can_Deserialise_ok_response_without_body__as_DeleteCard()
		{
			const string ResponseXml = "<?xml version=\"1.0\" encoding=\"iso-8859-1\"?><response status=\"ok\" version=\"1.2\" />";

			var response = new Response
			    {
			        StatusCode = HttpStatusCode.OK,
			        Body = ResponseXml
			    };

			var xmlSerializer = new ResponseDeserializer<DeleteCard>();
			var result = xmlSerializer.Deserialize(response);

			Assert.That(result, Is.Not.Null);
		}
	}
}
