using System;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using Newtonsoft.Json;
using SevenDigital.Api.Wrapper.Responses.Parsing.Exceptions;

namespace SevenDigital.Api.Wrapper.Responses.Parsing
{
	public class ResponseDeserializer
	{
		public T DeserializeResponse<T>(Response response, bool unwrapResponse) where T : class, new()
		{
			if (response.ContentTypeIsJson())
			{
				return ResponseFromJson<T>(response.Body);
			}

			return ResponseFromXmlText<T>(response.Body, unwrapResponse);
		}

		private T ResponseFromXmlText<T>(string responseBody, bool unwrapResponse) where T : class, new()
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

				using (var reader = MakePayloadXmlReader(doc, unwrapResponse))
				{
					return DeserialiseFromXmlReader<T>(reader);
				}
			}
		}

		private XmlReader MakePayloadXmlReader(XDocument doc, bool unwrapResponse)
		{
			if (!unwrapResponse)
			{
				return doc.CreateReader();
			}

			XElement responseNode;
			try
			{
				responseNode = doc.Descendants("response").First();
			}
			catch (InvalidOperationException e)
			{
				throw new UnexpectedXmlContentException(e);
			}

			var responsePayload = responseNode.FirstNode;
			if (responsePayload == null)
			{
				return null;
			}

			return responsePayload.CreateReader();
		}

		private static T DeserialiseFromXmlReader<T>(XmlReader xmlReader) where T : class, new()
		{
			if (xmlReader == null)
			{
				return new T();
			}

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
