using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Xml;
using FakeItEasy;
using NUnit.Framework;
using SevenDigital.Api.Wrapper.EndpointResolution;
using SevenDigital.Api.Wrapper.EndpointResolution.OAuth;
using SevenDigital.Api.Wrapper.Exceptions;
using SevenDigital.Api.Wrapper.Utility.Http;

namespace SevenDigital.Api.Wrapper.Unit.Tests.Utility.Http
{
	[TestFixture]
	public class EndpointResolverTests
	{
		private const string API_URL = "http://api.7digital.com/1.2";
		private readonly string _consumerKey = new AppSettingsCredentials().ConsumerKey;
		private IUrlResolver _urlResolver;
	    private EndpointResolver _endpointResolver;
		private IUrlSigner _urlSigner;

		[SetUp]
        public void Setup()
	    {
	    	_urlResolver = A.Fake<IUrlResolver>();
			_urlSigner = A.Fake<IUrlSigner>();
			_endpointResolver = new EndpointResolver(_urlResolver, _urlSigner, CredentialChecker.Instance.Credentials);
        }

		[Test]
		public void Should_fire_resolve_with_correct_values()
		{
			A.CallTo(() => _urlResolver.Resolve(A<Uri>.Ignored, A<string>.Ignored, A<Dictionary<string,string>>.Ignored))
				.Returns("<response status=\"ok\" version=\"1.2\" ><serviceStatus><serverTime>2011-03-04T08:10:29Z</serverTime></serviceStatus></response>");

			const string expectedMethod = "GET";
			var expectedHeaders = new Dictionary<string,string>();

			var endPointState = new EndPointInfo { Uri = "test", HttpMethod = expectedMethod, Headers = expectedHeaders };
			var expected = new Uri(string.Format("{0}/test?oauth_consumer_key={1}", API_URL, _consumerKey));

			A.CallTo(() => _urlSigner.SignUrl(A<string>.Ignored, A<string>.Ignored, A<string>.Ignored, null)).Returns(expected);

			_endpointResolver.HitEndpoint(endPointState);

			A.CallTo(() => _urlResolver
					.Resolve(A<Uri>.That.Matches(x => x.PathAndQuery == expected.PathAndQuery), expectedMethod, A<Dictionary<string,string>>.Ignored))
					.MustHaveHappened();
		}

		[Test]
		public void Should_return_xmlnode_if_valid_xml_received()
		{
			Given_a_urlresolver_that_returns_valid_xml();

			var response = _endpointResolver.HitEndpoint(new EndPointInfo());
            var hitEndpoint = new XmlDocument();
            hitEndpoint.LoadXml(response);
			Assert.That(hitEndpoint.HasChildNodes);
			Assert.That(hitEndpoint.SelectSingleNode("//serverTime"), Is.Not.Null);
		}

		private void Given_a_urlresolver_that_returns_valid_xml()
		{
			A.CallTo(() => _urlResolver.Resolve(A<Uri>.Ignored, A<string>.Ignored, A<Dictionary<string,string>>.Ignored)).Returns(
				"<response status=\"ok\" version=\"1.2\" ><serviceStatus><serverTime>2011-03-04T08:10:29Z</serverTime></serviceStatus></response>");
		}
	}
}