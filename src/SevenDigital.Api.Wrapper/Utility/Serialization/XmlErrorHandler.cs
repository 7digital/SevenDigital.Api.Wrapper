using System.Xml;
using System.Xml.Linq;
using SevenDigital.Api.Wrapper.Exceptions;

namespace SevenDigital.Api.Wrapper.Utility.Serialization
{
	public class XmlErrorHandler : IXmlErrorHandler
	{
		public void AssertError(XElement response)
		{
			var status = response.Attribute("status");
			string statusAttribute = (status == null)
				? response.Name.LocalName
				: status.Value;

			if (statusAttribute == "error")
			{
				string message = string.Format("An error has occured in the Api\n{0}", response.FirstNode);
				throw new ApiXmlException(message, response.FirstNode.ToString());
			}
		}

		public XElement GetResponseAsXml(string output)
		{
			XDocument xml;
			try
			{
				xml = XDocument.Parse(output);
			}
			catch (XmlException)
			{
				xml = GetResponseAsError(output);
			}
			return xml.Root;
		}

		private static XDocument GetResponseAsError(string text)
		{
			string errorXml = string.Format("<?xml version=\"1.0\" encoding=\"utf-8\" ?><response status=\"error\" version=\"1.2\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:noNamespaceSchemaLocation=\"http://api.7digital.com/1.2/static/7digitalAPI.xsd\" ><error code=\"9001\"><errorMessage>{0}</errorMessage></error></response>", text);
			try
			{
				return XDocument.Parse(errorXml);
			}
			catch (XmlException xmlEx)
			{
				throw new ApiXmlException("Could not parse Api response as xml\n" + text, xmlEx);
			}
		}
	}
}