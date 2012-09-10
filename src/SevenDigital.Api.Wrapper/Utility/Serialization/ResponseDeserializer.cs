using System;
using System.Net;
using System.Xml.Linq;
using SevenDigital.Api.Schema;
using SevenDigital.Api.Wrapper.Exceptions;
using SevenDigital.Api.Wrapper.Utility.Http;

namespace SevenDigital.Api.Wrapper.Utility.Serialization
{
	public class ResponseDeserializer<T> : IResponseDeserializer<T> where T : class
	{
		private const int DefaultErrorCode = 9001;

		public T Deserialize(Response response)
		{
			CheckResponse(response);
			return ParsedResponse(response);
		}

		private void CheckResponse(Response response)
		{
			if (response == null)
			{
				throw new ApiXmlException("No response");
			}

			if (string.IsNullOrEmpty(response.Body))
			{
				throw new ApiXmlException("No response body", response.StatusCode);
			}

			var startOfMessage = StartOfMessage(response.Body);
			var messageIsXml = IsXml(startOfMessage);

			if (messageIsXml && IsApiErrorResponse(startOfMessage))
			{
				var error = ParseError(response.Body);
				throw new ApiXmlException("Error response:\n" + response.Body, response.StatusCode, error);
			}

			if (IsServerError((int)response.StatusCode))
			{
				throw new ApiXmlException("Server error:\n" + response.Body, response.StatusCode);
			}

			if (!messageIsXml && response.StatusCode != HttpStatusCode.OK)
			{
				var error = new Error
					{
						Code = DefaultErrorCode,
						ErrorMessage = response.Body
					};
				throw new ApiXmlException("Error response:\n" + response.Body, response.StatusCode, error);
			}

			if (messageIsXml && !IsApiOkResponse(startOfMessage))
			{
				throw new ApiXmlException("No valid status found in response. Status must be one of 'ok' or 'error':\n" + response.Body, response.StatusCode);
			}
		}

		private static string StartOfMessage(string bodyMarkup)
		{
			int maxLength = Math.Min(bodyMarkup.Length, 512);
			return bodyMarkup.Substring(0, maxLength);
		}

		private bool IsXml(string bodyMarkup)
		{
			return bodyMarkup.Contains("<?xml");
		}

		private bool IsApiOkResponse(string bodyMarkup)
		{
			return bodyMarkup.Contains("<response") && bodyMarkup.Contains("status=\"ok\"");
		}

		private bool IsApiErrorResponse(string bodyMarkup)
		{
			return bodyMarkup.Contains("<response")  && bodyMarkup.Contains("status=\"error\"");
		}

		private bool IsServerError(int httpStatusCode)
		{
			return httpStatusCode >= 500;
		}

		private Error ParseError(string xml)
		{
			try
			{
				var xmlDoc = XDocument.Parse(xml);
				var responseNode = xmlDoc.FirstNode as XElement;
				var errorNode = responseNode.FirstNode as XElement;
				var errorMessage = errorNode.FirstNode as XElement;

				int errorCode = ReadErrorCode(errorNode);

				return new Error
				    {
						Code = errorCode,
				       	ErrorMessage = errorMessage.Value
				    };
			}
			catch(Exception ex)
			{
				return new Error
					{
						Code = DefaultErrorCode,
						ErrorMessage =  "XML error parse failed: " + ex
					};
			}
		}

		private int ReadErrorCode(XElement errorNode)
		{
			var attribute = errorNode.Attribute("code");
			return int.Parse(attribute.Value);
		}

		private static T ParsedResponse(Response response)
		{
			try
			{
				var deserializer = new StringDeserializer<T>();
				return deserializer.Deserialize(response.Body);
			}
			catch (Exception ex)
			{
				string message = string.Format("Error trying to deserialize xml response\n{0}", response.Body);
				throw new ApiXmlException(message, response.StatusCode, ex);
			}
		}
	}
}