
using System.Collections.Specialized;
using System.Xml;
using NUnit.Framework;
using Rhino.Mocks;
using SevenDigital.Api.Wrapper.Repository;
using SevenDigital.Api.Wrapper.Schema;

namespace SevenDigital.Api.Wrapper.Unit.Tests
{
	[TestFixture]
	public class FluentApiTests
	{
		[Test]
		public void Should_fire_endpointresolver_with_correct_endpoint_on_resolve()
		{
			var endpointResolver = MockRepository.GenerateStub<IEndpointResolver>();
			endpointResolver.Stub(x => x.HitEndpoint(null, null, null)).IgnoreArguments().Return(new XmlDocument());

			new FluentApi<Status>(endpointResolver).Resolve();

			endpointResolver.AssertWasCalled(x => x.HitEndpoint(Arg<string>.Is.Equal("status"), Arg<string>.Is.Anything, Arg<NameValueCollection>.Is.Anything));
		}

		[Test]
		public void Should_fire_endpointresolver_with_correct_methodname_on_resolve()
		{
			var endpointResolver = MockRepository.GenerateStub<IEndpointResolver>();
			endpointResolver.Stub(x => x.HitEndpoint(null, null, null)).IgnoreArguments().Return(new XmlDocument());

			new FluentApi<Status>(endpointResolver).WithMethod("POST").Resolve();

			endpointResolver.AssertWasCalled(x => x.HitEndpoint(Arg<string>.Is.Anything, Arg<string>.Is.Equal("POST"), Arg<NameValueCollection>.Is.Anything));
		}

		[Test]
		public void Should_fire_endpointresolver_with_correct_parameters_on_resolve()
		{
			var endpointResolver = MockRepository.GenerateStub<IEndpointResolver>();
			endpointResolver.Stub(x => x.HitEndpoint(null, null, null)).IgnoreArguments().Return(new XmlDocument());

			new FluentApi<Status>(endpointResolver).WithParameter("artistId", "123").Resolve();

			var expectedParameters = new NameValueCollection {{"artistId", "123"}};
			endpointResolver.AssertWasCalled(x => x.HitEndpoint(Arg<string>.Is.Anything, Arg<string>.Is.Anything, Arg<NameValueCollection>.Is.Equal(expectedParameters)));
		}
	}
}
