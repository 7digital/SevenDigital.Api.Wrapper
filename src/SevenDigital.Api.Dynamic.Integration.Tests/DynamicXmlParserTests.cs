using System.Collections.Generic;
using System.Collections.Specialized;
using NUnit.Framework;
using SevenDigital.Api.Wrapper.EndpointResolution;
using SevenDigital.Api.Wrapper.EndpointResolution.OAuth;
using SevenDigital.Api.Wrapper.Utility.Http;

namespace SevenDigital.Api.Dynamic.Integration.Tests {
	[TestFixture]
	public class DynamicXmlParserTests {
		private EndpointResolver _endpointResolver;

		[SetUp]
		public void SetUp() {
			_endpointResolver = new EndpointResolver(new HttpGetResolver(), new UrlSigner(), CredentialChecker.Instance.Credentials);
		}

		[Test]
		public void Can_deal_with_xml() {
			string xml = "<books><book id='1'><name>Book with 2 authors</name><authors><author id='1'>Billy</author></authors></book><book id='2'><name>The Bobbit</name></book></books>";

			dynamic dx = new DynamicXmlParser(xml);
			var condition = dx.book[1].name[0].Value;
			Assert.That(condition, Is.EqualTo("The Bobbit"));
		}

		[Test]
		public void Can_get_an_artist() {
			const string endpoint = "artist/details";

			var endPointInfo = new EndPointInfo { Uri = endpoint, Parameters = new Dictionary<string,string> { { "artistId", "1" } } };

			string xml = _endpointResolver.GetRawXml(endPointInfo);

			dynamic dx = new DynamicXmlParser(xml);

			var name = dx.artist[0].name.Value;
			var sortName = dx.artist[0].sortName.Value;
			var url = dx.artist[0].url.Value;

			Assert.That(name, Is.EqualTo("Keane"));
			Assert.That(sortName, Is.EqualTo("Keane"));
			Assert.That(url, Is.StringStarting("http://www.7digital.com/artists/keane/"));
		}

		[Test]
		public void Can_get_an_artists_releases() {
			const string endpoint = "artist/releases";

			var endPointInfo = new EndPointInfo { Uri = endpoint, Parameters =  new Dictionary<string,string> { { "artistId", "1" } } };

			string xml = _endpointResolver.GetRawXml(endPointInfo);

			dynamic dx = new DynamicXmlParser(xml);

			var name = dx.releases.release[0].title.Value;
			var secondName = dx.releases.release[1].title.Value;

			Assert.That(name, Is.EqualTo("Perfect Symmetry"));
			Assert.That(secondName, Is.EqualTo("Night Train"));
		}
	}
}
