using System.Xml;
using System.Xml.Serialization;

namespace SevenDigital.Api.Wrapper.Requests.Serializing
{
	public class XmlPayloadSerializer : IPayloadSerializer
	{
		public PayloadFormat Handles
		{
			get { return PayloadFormat.Xml; }
		}

		public string ContentType
		{
			get { return "application/xml"; }
		}

		public string Serialize<TPayload>(TPayload payload) where TPayload : class
		{
			var blankNamespace = new XmlSerializerNamespaces();
			blankNamespace.Add("", "");

			var xmlSerializer = new XmlSerializer(typeof(TPayload), "");

			using (var stringWriter = new Utf8StringWriter())
			{
				using (var xml = XmlWriter.Create(stringWriter))
				{
					xmlSerializer.Serialize(xml, payload, blankNamespace);
				}
				return stringWriter.ToString();
			}
		}
	}
}