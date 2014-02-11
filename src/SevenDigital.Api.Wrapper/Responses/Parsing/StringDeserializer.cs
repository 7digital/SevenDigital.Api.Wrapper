using System;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using SevenDigital.Api.Wrapper.Responses.Parsing.Exceptions;

namespace SevenDigital.Api.Wrapper.Responses.Parsing
{
	public class StringDeserializer<T> where T : class
	{
		public T Deserialize(string response)
		{
			using (var reader = new StringReader(response))
			{
				XDocument doc;
				try {
					doc = XDocument.Load(reader);
				} catch (XmlException e) {
					throw new NonXmlContentException(e);
				}

				XElement responseNode;
				try {
					responseNode = doc.Descendants("response").First();
				} catch (InvalidOperationException e) {
					throw new UnexpectedXmlContentException(e);
				}

				var responsePayload = responseNode.FirstNode;
				if (responsePayload == null)
				{
					return (T) Activator.CreateInstance(typeof (T));
				}

				using (var payloadReader = responsePayload.CreateReader())
				{
					var ser = new XmlSerializer(typeof(T));
					try {
						return (T) ser.Deserialize(payloadReader);
					} catch (InvalidOperationException ex) {
						throw new UnexpectedXmlContentException(ex);
					}
				}
			}
		}
	}
}
