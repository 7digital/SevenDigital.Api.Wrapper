using NUnit.Framework;
using SevenDigital.Api.Schema.ArtistEndpoint;
using SevenDigital.Api.Wrapper.Requests.Serializing;

namespace SevenDigital.Api.Wrapper.Unit.Tests.Requests.Serializing
{
	[TestFixture]
	public class XmlSerializerTest
	{
		[Test]
		public void SHould_serialize_artist_as_expected()
		{
			const string expectedXmlOutput = "<?xml version=\"1.0\" encoding=\"utf-8\"?><artist id=\"143451\"><name>MGMT</name><appearsAs>MGMT</appearsAs><image>http://cdn.7static.com/static/img/artistimages/00/001/434/0000143451_150.jpg</image><url>http://www.7digital.com/artist/mgmt/?partner=1401</url></artist>";
			
			var artist = new Artist
			{
				AppearsAs = "MGMT",
				Name = "MGMT",
				Id = 143451,
				Image = "http://cdn.7static.com/static/img/artistimages/00/001/434/0000143451_150.jpg",
				Url = "http://www.7digital.com/artist/mgmt/?partner=1401"
			};

			var xml = artist.ToXml();

			Assert.That(xml, Is.EqualTo(expectedXmlOutput));
		}
	}
}