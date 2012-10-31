using System;
using System.Net;
using System.Xml.Linq;
using SevenDigital.Api.Schema;
using SevenDigital.Api.Wrapper.EndpointResolution;
using SevenDigital.Api.Wrapper.Exceptions;
using SevenDigital.Api.Wrapper.Utility.Http;

namespace SevenDigital.Api.Wrapper.Utility.Serialization
{
	public class ResponseDeserializer<T> : IResponseDeserializer<T> where T : class
	{
		private readonly IApiResponseDetector _apiResponseDetector;
		private const int DefaultErrorCode = 9001;

		public ResponseDeserializer()
			: this(EssentialDependencyCheck<IApiResponseDetector>.Instance)
		{
		}

		public ResponseDeserializer(IApiResponseDetector apiResponseDetector)
		{
			_apiResponseDetector = apiResponseDetector;
		}

		public T Deserialize(Response response)
		{
			CheckResponse(response);
			return ParsedResponse(response);
		}

		private void CheckResponse(Response response)
		{
			if (response == null)
			{
				throw new ArgumentNullException("response");
			}

			if (string.IsNullOrEmpty(response.Body))
			{
				throw CreateNonXmlResponseException(response);
			}

			var messageIsXml = _apiResponseDetector.IsXml(response.Body);

			if (messageIsXml && _apiResponseDetector.IsApiErrorResponse(response.Body))
			{
				var error = ParseError(response.Body);
				throw new ApiXmlException("Error response:\n" + response.Body, response.StatusCode, error);
			}

			if (_apiResponseDetector.IsServerError((int)response.StatusCode))
			{
				if (!messageIsXml)
				{
					throw CreateNonXmlResponseException(response);
				}

				var error = ParseError(response.Body);
				throw new ApiXmlException("Server error:\n" + response.Body, response.StatusCode, error);
			}

			if (!messageIsXml && response.StatusCode != HttpStatusCode.OK)
			{
				throw CreateNonXmlResponseException(response);
			}

			if (messageIsXml && !_apiResponseDetector.IsApiOkResponse(response.Body))
			{
				throw new ApiXmlException("No valid status found in response. Status must be one of 'ok' or 'error':\n" + response.Body, response.StatusCode);
			}
		}

		private static NonXmlResponseException CreateNonXmlResponseException(Response response)
		{
			var nonXmlResponseException = new NonXmlResponseException();
			nonXmlResponseException.ResponseBody = response.Body;
			nonXmlResponseException.StatusCode = response.StatusCode;
			return nonXmlResponseException;
		}

		private Error ParseError(string xml)
		{
			try
			{
				var xmlDoc = XDocument.Parse(xml);
				var responseNode = xmlDoc.FirstNode as XElement;
				var errorNode = responseNode.FirstNode as XElement;
				var errorMessage = errorNode.FirstNode as XElement;

				var errorCode = ReadErrorCode(errorNode);

				return new Error
					{
						Code = errorCode,
						ErrorMessage = errorMessage.Value
					};
			}
			catch (Exception ex)
			{
				return new Error
					{
						Code = DefaultErrorCode,
						ErrorMessage = "XML error parse failed: " + ex
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
				var nonXmlResponseException = new NonXmlResponseException(NonXmlResponseException.DEFAULT_ERROR_MESSAGE, ex);
				nonXmlResponseException.ResponseBody = response.Body;
				nonXmlResponseException.StatusCode = response.StatusCode;
				throw nonXmlResponseException;
			}
		}
	}

}