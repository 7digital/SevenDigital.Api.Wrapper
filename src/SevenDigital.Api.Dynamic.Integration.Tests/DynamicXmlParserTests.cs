using System.Collections.Generic;
using System.Xml.Linq;
using NUnit.Framework;
using SevenDigital.Api.Wrapper;
using SevenDigital.Api.Wrapper.EndpointResolution;
using SevenDigital.Api.Wrapper.EndpointResolution.OAuth;
using SevenDigital.Api.Wrapper.Utility.Http;

namespace SevenDigital.Api.Dynamic.Integration.Tests {
	[TestFixture]
	public class DynamicXmlParserTests {
		private EndpointResolver _endpointResolver;

		[SetUp]
		public void SetUp() {
			IOAuthCredentials oAuthCredentials = EssentialDependencyCheck<IOAuthCredentials>.Instance;
			IApiUri apiUri = EssentialDependencyCheck<IApiUri>.Instance;
			var httpGetResolver = new HttpGetResolver();
			var urlSigner = new UrlSigner();

			_endpointResolver = new EndpointResolver(httpGetResolver, urlSigner, oAuthCredentials, apiUri);
		}

		[Test]
		public void Can_get_an_artist() {
			const string endpoint = "artist/details";

			var endPointInfo = new EndPointInfo { Uri = endpoint, Parameters = new Dictionary<string,string> { { "artistId", "1" } } };

			string xml = _endpointResolver.HitEndpoint(endPointInfo);

			dynamic dx = new DynamicXmlParser(XDocument.Parse(xml));

			var name = dx.artist[0].name.value;
			var sortName = dx.artist[0].sortName.value;
			var url = dx.artist[0].url.value;

			Assert.That(name, Is.EqualTo("Keane"));
			Assert.That(sortName, Is.EqualTo("Keane"));
			Assert.That(url, Is.StringStarting("http://www.7digital.com/artists/keane/"));
		}

		[Test]
		public void Can_get_an_artists_releases() {
			const string endpoint = "artist/releases";

			var endPointInfo = new EndPointInfo { Uri = endpoint, Parameters =  new Dictionary<string,string> { { "artistId", "1" } } };

			string xml = _endpointResolver.HitEndpoint(endPointInfo);

			dynamic dx = new DynamicXmlParser(XDocument.Parse(xml));

			var name = dx.releases.release[0].title.value;
			var secondName = dx.releases.release[1].title.value;

            Assert.That(name, Is.EqualTo("Night Train"));
            Assert.That(secondName, Is.EqualTo("A Bad Dream"));
		}
	}
}
