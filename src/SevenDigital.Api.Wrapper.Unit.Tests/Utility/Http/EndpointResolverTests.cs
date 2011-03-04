using System;
using System.Configuration;
using System.Net;
using System.Xml;
using NUnit.Framework;
using Rhino.Mocks;
using SevenDigital.Api.Wrapper.Repository;
using SevenDigital.Api.Wrapper.Utility.Http;

namespace SevenDigital.Api.Wrapper.Unit.Tests.Utility.Http
{
	[TestFixture]
	public class EndpointResolverTests
	{
		private readonly string _apiUrl = ConfigurationManager.AppSettings["Wrapper.BaseUrl"];
		private readonly string _consumerKey = ConfigurationManager.AppSettings["Wrapper.ConsumerKey"];
		private readonly IUrlResolver _urlResolver = MockRepository.GenerateMock<IUrlResolver>();

		[Test]
		public void Should_fire_resolve_with_correct_values()
		{
			Given_a_urlresolver_that_returns_valid_xml();

			var endpointResolver = new EndpointResolver(_urlResolver);
			endpointResolver.HitEndpoint("test", "GET", null);
			var expected = new Uri(string.Format("{0}/test?oauth_consumer_key={1}", _apiUrl, _consumerKey));
			_urlResolver.AssertWasCalled(x=>x.Resolve(Arg<Uri>.Is.Equal(expected), Arg<string>.Is.Equal("GET"), Arg<WebHeaderCollection>.Is.Equal(new WebHeaderCollection())));
		}

		[Test]
		public void Should_return_xmlnode_if_valid_xml_received()
		{
			Given_a_urlresolver_that_returns_valid_xml();

			var endpointResolver = new EndpointResolver(_urlResolver);
			XmlNode hitEndpoint = endpointResolver.HitEndpoint("", "", null);
			Assert.That(hitEndpoint.HasChildNodes);
			Assert.That(hitEndpoint.SelectSingleNode("//serverTime"), Is.Not.Null);
		}

		[Test]
		public void Should_throw_api_exception_with_correct_error_if_error_xml_received()
		{
			_urlResolver.Stub(x => x.Resolve(null, "", null))
				.IgnoreArguments()
				.Return("<response status=\"error\" version=\"1.2\"><error code=\"1001\"><errorMessage>Missing parameter \"tags\".</errorMessage></error></response>");

			var endpointResolver = new EndpointResolver(_urlResolver);
			var apiException = Assert.Throws<ApiException>(() => endpointResolver.HitEndpoint("", "", null));
			Assert.That(apiException.Message, Is.EqualTo("An error has occured in the Api"));
			Assert.That(apiException.Error.Code, Is.EqualTo(1001));
			Assert.That(apiException.Error.ErrorMessage, Is.EqualTo("Missing parameter \"tags\"."));
		}

		private void Given_a_urlresolver_that_returns_valid_xml()
		{
			_urlResolver.Stub(x => x.Resolve(null, "", null))
				.IgnoreArguments()
				.Return("<response status=\"ok\" version=\"1.2\" ><serviceStatus><serverTime>2011-03-04T08:10:29Z</serverTime></serviceStatus></response>");
		}
	}
}