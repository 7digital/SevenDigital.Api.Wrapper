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
			string normalizedUrl;
			string normalizedRequestParameters;

			var oAuthSignatureParameters = new OAuthSignatureParameters();

			string generateSignature = oAuthBase.GenerateSignature(oAuthSignatureParameters, out normalizedUrl, out normalizedRequestParameters);

		
		}
	}
}
