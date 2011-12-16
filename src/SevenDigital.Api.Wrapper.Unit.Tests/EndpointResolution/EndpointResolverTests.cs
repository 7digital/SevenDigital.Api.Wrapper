using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using FakeItEasy;
using NUnit.Framework;
using SevenDigital.Api.Schema.Attributes;
using SevenDigital.Api.Wrapper.EndpointResolution;
using SevenDigital.Api.Wrapper.EndpointResolution.OAuth;
using SevenDigital.Api.Wrapper.Utility.Http;

namespace SevenDigital.Api.Wrapper.Unit.Tests.EndpointResolution
{
	[TestFixture]
	public class EndpointResolverTests
	{
		private EndpointResolver _endpointResolver;

		[SetUp]
		public void Setup()
		{
			var urlResolver = A.Fake<IUrlResolver>();
			A.CallTo(() => urlResolver.Resolve(A<Uri>.Ignored, A<string>.Ignored, A<Dictionary<string, string>>.Ignored))
				.Returns(string.Empty);

			var apiUri = A.Fake<IApiUri>();
			A.CallTo(() => apiUri.Uri)
				.Returns("http://uri/");

			_endpointResolver = new EndpointResolver(urlResolver, new UrlSigner(), new AppSettingsCredentials(), apiUri);
		}

		[Test]
		public void should_substitue_route_parameters()
		{
			var endpointInfo = new EndPointInfo
			{
				Uri = "something/{route}",
				Parameters = new Dictionary<string, string>
						{
							{"route","routevalue"}
						}
			};

			var result = _endpointResolver.ConstructEndpoint(endpointInfo);

			Assert.That(result,Is.StringContaining("something/routevalue"));
		}

		[Test]
		public void should_substitue_multiple_route_parameters()
		{
			var endpointInfo = new EndPointInfo
			{
				Uri = "something/{firstRoute}/{secondRoute}/thrid/{thirdRoute}",
				Parameters = new Dictionary<string, string>
						{
							{"firstRoute" , "firstValue"},
							{"secondRoute","secondValue"},
							{"thirdRoute" , "thirdValue"}
							
						}
			};

			var result = _endpointResolver.ConstructEndpoint(endpointInfo);

			Assert.That(result, Is.StringContaining("something/firstvalue/secondvalue/thrid/thirdvalue"));
		}

		[Test]
		public void routes_should_be_case_insensitive()
		{
			var endpointInfo = new EndPointInfo
			{
				Uri = "something/{firstRoUte}/{secOndrouTe}/thrid/{tHirdRoute}",
				Parameters = new Dictionary<string, string>
						{
							{"firstRoute" , "firstValue"},
							{"secondRoute","secondValue"},
							{"thirdRoute" , "thirdValue"}
							
						}
			};

			var result = _endpointResolver.ConstructEndpoint(endpointInfo);

			Assert.That(result, Is.StringContaining("something/firstvalue/secondvalue/thrid/thirdvalue"));
		}

		[Test]
		public void should_handle_dashes_and_numbers()
		{
			var endpointInfo = new EndPointInfo
			{
				Uri = "something/{route-66}",
				Parameters = new Dictionary<string, string>
						{
							{"route-66","routevalue"}
						}
			};

			var result = _endpointResolver.ConstructEndpoint(endpointInfo);

			Assert.That(result, Is.StringContaining("something/routevalue"));
		}

	}
}
