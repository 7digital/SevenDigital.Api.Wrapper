using System.Xml;
using System.Xml.XPath;

namespace SevenDigital.Api.Wrapper.Utility.Serialization
{
	public interface ISerializer<T>
	{
		T DeSerialize(XmlNode document);
		IXPathNavigable Serialize(T serializableObject);
	}
}