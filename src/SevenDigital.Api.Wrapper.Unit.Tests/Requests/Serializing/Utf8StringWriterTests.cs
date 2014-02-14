using System.Xml;
using NUnit.Framework;
using SevenDigital.Api.Wrapper.Requests.Serializing;

namespace SevenDigital.Api.Wrapper.Unit.Tests.Requests.Serializing
{
	[TestFixture]
	public class Utf8StringWriterTests
	{
		[Test]
		public void Should_set_encoding_to_utf8()
		{
			using (var utf8StringWriter = new Utf8StringWriter())
			{
				using (var xml = XmlWriter.Create(utf8StringWriter))
				{
					xml.WriteStartDocument();
				}
				Assert.That(utf8StringWriter.ToString(), Is.StringContaining("encoding=\"utf-8\""));
			}
		}
	}
}