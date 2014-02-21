using NUnit.Framework;
using SevenDigital.Api.Wrapper.Requests.Serializing;

namespace SevenDigital.Api.Wrapper.Unit.Tests.Requests.Serializing
{
	[TestFixture]
	public class JsonTransferContentTypeTest
	{
		private JsonTransferContentType _transferContentType;

		[SetUp]
		public void SetUp()
		{
			_transferContentType = new JsonTransferContentType();
		}

		[Test]
		public void Should_have_correct_contenttype()
		{
			Assert.That(_transferContentType.ContentType, Is.EqualTo("application/json"));
		}

		[Test]
		[Ignore("Not implemented yet")]
		public void SHould_serialize_artist_as_expected()
		{
			
		}
	}
}