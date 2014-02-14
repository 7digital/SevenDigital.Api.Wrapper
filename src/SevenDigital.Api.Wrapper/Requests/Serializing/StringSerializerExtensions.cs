using System.Xml;
using System.Xml.Serialization;

namespace SevenDigital.Api.Wrapper.Requests.Serializing
{
	public static class StringSerializerExtensions
	{
		public static string ToXml<T>(this T entity) where T : class
		{
			var blankNamespace = new XmlSerializerNamespaces();
			blankNamespace.Add("", ""); 

			var xmlSerializer = new XmlSerializer(typeof (T), "");

			using (var stringWriter = new Utf8StringWriter())
			{
				using (var xml = XmlWriter.Create(stringWriter))
				{
					xmlSerializer.Serialize(xml, entity, blankNamespace);
				}
				return stringWriter.ToString();
			}
		}
	}
}