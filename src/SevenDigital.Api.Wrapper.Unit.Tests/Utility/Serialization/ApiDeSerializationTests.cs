using NUnit.Framework;
using SevenDigital.Api.Schema;
using SevenDigital.Api.Wrapper.Exceptions;
using SevenDigital.Api.Wrapper.Utility.Serialization;

namespace SevenDigital.Api.Wrapper.Unit.Tests.Utility.Serialization
{
    [TestFixture]
    public class ApiDeSerializationTests
    {
        [Test]
        public void Should_throw_api_exception_with_correct_error_if_error_xml_received()
        {
            var apiXmlDeSerializer = new ApiXmlDeSerializer<Status>(null, new XmlErrorHandler());

            var apiException = Assert.Throws<ApiXmlException>(() => apiXmlDeSerializer.DeSerialize(string.Empty));
            Assert.That(apiException.Message, Is.StringContaining("An error has occured in the Api"));
            Assert.That(apiException.Error.Code, Is.EqualTo(9001));
        }
    }
}
