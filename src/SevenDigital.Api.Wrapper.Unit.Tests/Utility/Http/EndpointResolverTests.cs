using System;
using System.Configuration;
using System.Net;
using System.Xml;
using FakeItEasy;
using NUnit.Framework;
using SevenDigital.Api.Wrapper.EndpointResolution;
using SevenDigital.Api.Wrapper.Exceptions;
using SevenDigital.Api.Wrapper.Utility.Http;

namespace SevenDigital.Api.Wrapper.Unit.Tests.Utility.Http
{
	[TestFixture]
	public class EndpointResolverTests
	{
		private readonly string _apiUrl = ConfigurationManager.AppSettings["Wrapper.BaseUrl"];
		private readonly string _consumerKey = ConfigurationManager.AppSettings["Wrapper.ConsumerKey"];
		private IUrlResolver _urlResolver = A.Fake<IUrlResolver>();

		[Test]
		public void Should_fire_resolve_with_correct_values()
		{
			A.CallTo(() => _urlResolver.Resolve(A<Uri>.Ignored, A<string>.Ignored, A<WebHeaderCollection>.Ignored))
				.Returns("<response status=\"ok\" version=\"1.2\" ><serviceStatus><serverTime>2011-03-04T08:10:29Z</serverTime></serviceStatus></response>");

			var endpointResolver = new EndpointResolver(_urlResolver);
			const string expectedMethod = "GET";
			var expectedHeaders = new WebHeaderCollection();

			var endPointState = new EndPointState() {Uri = "test", HttpMethod = expectedMethod, Headers = expectedHeaders};
			var expected = new Uri(string.Format("{0}/test?oauth_consumer_key={1}", _apiUrl, _consumerKey));

			endpointResolver.HitEndpoint(endPointState);

			A.CallTo(() => _urlResolver.Resolve(A<Uri>.That.Matches(x => x.PathAndQuery == expected.PathAndQuery), expectedMethod, A<WebHeaderCollection>.Ignored))
				.MustHaveHappened();
		}

		[Test]
		public void Should_return_xmlnode_if_valid_xml_received()
		{
			Given_a_urlresolver_that_returns_valid_xml();

			var endpointResolver = new EndpointResolver(_urlResolver);
			XmlNode hitEndpoint = endpointResolver.HitEndpoint(new EndPointState());
			Assert.That(hitEndpoint.HasChildNodes);
			Assert.That(hitEndpoint.SelectSingleNode("//serverTime"), Is.Not.Null);
		}

		[Test]
		public void Should_throw_api_exception_with_correct_error_if_error_xml_received()
		{
			_urlResolver = A.Fake<IUrlResolver>();

			A.CallTo(() => _urlResolver.Resolve(A<Uri>.Ignored, A<string>.Ignored, A<WebHeaderCollection>.Ignored))
				.Returns(
				"<response status=\"error\" version=\"1.2\"><error code=\"1001\"><errorMessage>Missing parameter \"tags\".</errorMessage></error></response>");

			var endpointResolver = new EndpointResolver(_urlResolver);
			var apiException = Assert.Throws<ApiXmlException>(() => endpointResolver.HitEndpoint(new EndPointState()));
			Assert.That(apiException.Message, Is.EqualTo("An error has occured in the Api, see Error property for details"));
			Assert.That(apiException.Error.Code, Is.EqualTo(1001));
			Assert.That(apiException.Error.ErrorMessage, Is.EqualTo("Missing parameter \"tags\"."));
		}

		private void Given_a_urlresolver_that_returns_valid_xml()
		{
			A.CallTo(() => _urlResolver.Resolve(A<Uri>.Ignored, A<string>.Ignored, A<WebHeaderCollection>.Ignored)).Returns(
				"<response status=\"ok\" version=\"1.2\" ><serviceStatus><serverTime>2011-03-04T08:10:29Z</serverTime></serviceStatus></response>");
		}
	}
}