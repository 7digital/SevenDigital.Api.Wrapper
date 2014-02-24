using NUnit.Framework;
using SevenDigital.Api.Schema.ArtistEndpoint;
using SevenDigital.Api.Wrapper.Requests.Serializing;

namespace SevenDigital.Api.Wrapper.Unit.Tests.Requests.Serializing
{
	[TestFixture]
	public class JsonTransferContentTypeTest
	{
		private JsonPayloadSerializer _payloadSerializer;

		[SetUp]
		public void SetUp()
		{
			_payloadSerializer = new JsonPayloadSerializer();
		}

		[Test]
		public void Should_have_correct_contenttype()
		{
			Assert.That(_payloadSerializer.ContentType, Is.EqualTo("application/json"));
		}

		[Test]
		public void Should_serialize_artist_as_expected()
		{
			const string expected = "{\"id\":143451,\"name\":\"MGMT\",\"sortName\":null,\"appearsAs\":\"MGMT\",\"image\":\"http://cdn.7static.com/static/img/artistimages/00/001/434/0000143451_150.jpg\",\"url\":\"http://www.7digital.com/artist/mgmt/?partner=1401\"}";
			
			var artist = new Artist
			{
				AppearsAs = "MGMT",
				Name = "MGMT",
				Id = 143451,
				Image = "http://cdn.7static.com/static/img/artistimages/00/001/434/0000143451_150.jpg",
				Url = "http://www.7digital.com/artist/mgmt/?partner=1401"
			};

			var json = _payloadSerializer.Serialize(artist);

			Assert.That(json, Is.EqualTo(expected));
		}
	}
}