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
			return ResponseFromXmlText<T>(response.Body);
		}

		private T ResponseFromXmlText<T>(string responseBody)
		{
			using (var stringReader = new StringReader(responseBody))
			{
				XDocument doc;
				try
				{
					doc = XDocument.Load(stringReader);
				}
				catch (XmlException e)
				{
					throw new NonXmlContentException(e);
				}

				var xmlReader = doc.CreateReader();
				return ResponseFromXmlDoc<T>(xmlReader);
			}
		}

		private static T ResponseFromXmlDoc<T>(XmlReader xmlReader)
		{
			var deserializer = new XmlSerializer(typeof(T));
			try
			{
				return (T)deserializer.Deserialize(xmlReader);
			}
			catch (InvalidOperationException ex)
			{
				throw new UnexpectedXmlContentException(ex);
			}
		}

		private T ResponseFromJson<T>(string responseBody)
		{
			try
			{
				return JsonConvert.DeserializeObject<T>(responseBody);
			}
			catch (JsonReaderException jrEx)
			{
				throw new JsonParseFailedException(jrEx);
			}
		}
	}
}
