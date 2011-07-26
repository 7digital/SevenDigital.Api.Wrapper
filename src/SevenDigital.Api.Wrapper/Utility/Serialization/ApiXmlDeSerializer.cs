using System;
using System.Xml;
using System.Xml.Linq;
using SevenDigital.Api.Wrapper.Exceptions;

namespace SevenDigital.Api.Wrapper.Utility.Serialization
{
    public class ApiXmlDeSerializer<T> : IDeSerializer<T> where T : class
    {
        private readonly IDeSerializer<T> _deSerializer;

        public ApiXmlDeSerializer(IDeSerializer<T> deSerializer)
        {
            _deSerializer = deSerializer;
        }

        public T DeSerialize(string response)
        {
            try {
                var responseNode = GetResponseAsXml(response);
                AssertError(responseNode);
                var resourceNode = responseNode.FirstNode.ToString();
                return _deSerializer.DeSerialize(resourceNode);
            } catch (Exception e) {
                if (e is ApiXmlException)
                    throw;
                throw new ApplicationException("Internal error while deserializing response " + response, e);
            }
        }


        private static void AssertError(XElement response)
        {
            var status = response.Attribute("status");
            string statusAttribute = status == null ? response.Name.LocalName : status.Value;
            if (statusAttribute == "error")
                throw new ApiXmlException("An error has occured in the Api, see Error property for details", response.FirstNode.ToString());
        }

        private static XElement GetResponseAsXml(string output)
        {
            XDocument xml;
            try
            {
                xml = XDocument.Parse(output);
            }
            catch (XmlException)
            {
                string errorXml = string.Format("<?xml version=\"1.0\" encoding=\"utf-8\" ?><response status=\"error\" version=\"1.2\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:noNamespaceSchemaLocation=\"http://api.7digital.com/1.2/static/7digitalAPI.xsd\" ><error code=\"9001\"><errorMessage>{0}</errorMessage></error></response>", output);
                xml = XDocument.Parse(errorXml);
            }
            return xml.Element("response");
        }
    }
}