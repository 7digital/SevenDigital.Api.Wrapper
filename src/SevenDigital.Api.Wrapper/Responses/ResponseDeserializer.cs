using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using Newtonsoft.Json;
using SevenDigital.Api.Wrapper.Responses.Parsing.Exceptions;

namespace SevenDigital.Api.Wrapper.Responses
{
	public class ResponseDeserializer
	{
		public T ResponseAs<T>(Response response)
		{
			if (response.ContentTypeIsJson())
			{
				return ResponseFromJson<T>(response.Body);
			}
			return ResponseFromXml<T>(response.Body);
		}

		private T ResponseFromXml<T>(string responseBody)
		{
			using (var reader = new StringReader(responseBody))
			{
				try
				{
					XDocument doc = XDocument.Load(reader);
					var ser = new XmlSerializer(typeof(T));
					try
					{
						return (T)ser.Deserialize(doc.CreateReader());
					}
					catch (InvalidOperationException ex)
					{
						throw new UnexpectedXmlContentException(ex);
					}
				}
				catch (XmlException e)
				{
					throw new NonXmlContentException(e);
				}
			}
		}

		private T ResponseFromJson<T>(string responseBody)
		{
			return JsonConvert.DeserializeObject<T>(responseBody);
		}
	}
}
