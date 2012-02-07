using System;
using System.Collections.Generic;
using System.Linq;
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

		    string [] titles = Enumerable.ToArray(Enumerable.Select<dynamic, string>(dx.releases.release, (Func<dynamic, string>) (r => r.title.value)));

		    foreach (var title in titles) {
		        Console.WriteLine(title);
		    }

            Assert.That(titles, Has.Member("Night Train").And.Member("Perfect Symmetry"));
		}
	}
}
