using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace SevenDigital.Api.Wrapper.Serialization
{
	public class StringDeserializer<T> where T : class
	{
		public T Deserialize(string response)
		{		
			using (var reader = new StringReader(response))
			{
				XDocument doc = XDocument.Load(reader);
				var responseNode = doc.Descendants("response").First();

				var responsePayload = responseNode.FirstNode;
				if (responsePayload == null)
				{
					return (T) Activator.CreateInstance(typeof (T));
				}

				using (var payloadReader = responsePayload.CreateReader())
				{
					var ser = new XmlSerializer(typeof(T));
					return (T) ser.Deserialize(payloadReader);
				}
			}
		}
	}
}
