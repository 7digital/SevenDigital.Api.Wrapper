using System.Xml.Linq;

namespace SevenDigital.Api.Wrapper.Utility.Serialization
{
	public interface IXmlErrorHandler
	{
		void AssertError(XElement response);
		XElement GetResponseAsXml(string output);
	}
}