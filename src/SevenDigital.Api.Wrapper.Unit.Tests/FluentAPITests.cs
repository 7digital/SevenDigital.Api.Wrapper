using System;
using System.Linq.Expressions;
using System.Xml;
using FakeItEasy;
using NUnit.Framework;
using SevenDigital.Api.Wrapper.EndpointResolution;
using SevenDigital.Api.Wrapper.Schema;

namespace SevenDigital.Api.Wrapper.Unit.Tests
{
	[TestFixture]
	public class FluentApiTests
	{
		[Test]
		public void Should_fire_endpointresolver_with_correct_endpoint_on_resolve()
		{
			var endpointResolver = A.Fake<IEndpointResolver>();
			A.CallTo(() => endpointResolver.HitEndpoint(A<EndPointInfo>.Ignored)).Returns(new XmlDocument());

			new FluentApi<Status>(endpointResolver).Resolve();

			Expression<Func<XmlNode>> callWithEndpointStatus = 
				() => endpointResolver.HitEndpoint(A<EndPointInfo>.That.Matches(x => x.Uri == "status"));

			A.CallTo(callWithEndpointStatus).MustHaveHappened(Repeated.Exactly.Once);
		}

		[Test]
		public void Should_fire_endpointresolver_with_correct_methodname_on_resolve()
		{
			var endpointResolver = A.Fake<IEndpointResolver>();
			A.CallTo(() => endpointResolver.HitEndpoint(A<EndPointInfo>.Ignored)).Returns(new XmlDocument());

			new FluentApi<Status>(endpointResolver).WithMethod("POST").Resolve();

			Expression<Func<XmlNode>> callWithMethodPost = 
				() => endpointResolver.HitEndpoint(A<EndPointInfo>.That.Matches(x => x.HttpMethod == "POST"));

			A.CallTo(callWithMethodPost).MustHaveHappened(Repeated.Exactly.Once);
		}

		[Test]
		public void Should_fire_endpointresolver_with_correct_parameters_on_resolve()
		{
			var endpointResolver = A.Fake<IEndpointResolver>();
			A.CallTo(() => endpointResolver.HitEndpoint(A<EndPointInfo>.Ignored)).Returns(new XmlDocument());
			
			new FluentApi<Status>(endpointResolver).WithParameter("artistId", "123").Resolve();

			Expression<Func<XmlNode>> callWithArtistId123 = 
				() => endpointResolver.HitEndpoint(A<EndPointInfo>.That.Matches(x => x.Parameters["artistId"] == "123"));

			A.CallTo(callWithArtistId123).MustHaveHappened();

		}
	}
}
