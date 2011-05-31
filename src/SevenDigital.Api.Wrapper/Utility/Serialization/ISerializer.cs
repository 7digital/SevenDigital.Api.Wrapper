using System.Xml.XPath;

namespace SevenDigital.Api.Wrapper.Utility.Serialization
{
	public interface ISerializer<T>
	{
        IXPathNavigable Serialize(T serializableObject);
	}
}