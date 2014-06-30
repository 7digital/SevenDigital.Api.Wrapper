using System;
using NUnit.Framework;
using SevenDigital.Api.Schema.Artists;

namespace SevenDigital.Api.Schema.Unit.Tests
{
	[TestFixture]
	public class ApiEndpointTypeGeneratorTests
	{
		[Test]
		public void Should_be_able_to_find_a_type_from_endpoint_attribute()
		{
			Type generatedType = new ApiEndpointTypeGenerator().GenerateType("artist/releases");
			Assert.That(Activator.CreateInstance(generatedType), Is.TypeOf<ArtistReleases>());
		}

		[Test]
		public void Should_throw_argumentexception_if_cant_find_endpoint()
		{
			var argumentException = Assert.Throws<ArgumentException>(() => new ApiEndpointTypeGenerator().GenerateType("fartistreleases"));
			Assert.That(argumentException.Message, Is.EqualTo("Could not find endpoint defined with this name"));
		}
	}
}
