using System.Collections.Generic;
using System.Linq;
using System.Text;
using FakeItEasy;
using NUnit.Framework;
using SevenDigital.Api.Wrapper.EndpointResolution.Authorization;

namespace SevenDigital.Api.Wrapper.Unit.Tests.EndpointResolution.Authorization
{
	[TestFixture]
	public class OAuthTests
	{
		[Test, Ignore]
		public void Should_generate_signature()
		{
			var hashComputer = A.Fake<IHashComputer>();
			
			var oAuthBase = new OAuthBase(hashComputer);
		}
	}
}
