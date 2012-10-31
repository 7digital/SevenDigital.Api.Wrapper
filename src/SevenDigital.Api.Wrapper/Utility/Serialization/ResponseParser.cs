using System;
using System.Net;
using System.Xml.Linq;
using SevenDigital.Api.Schema;
using SevenDigital.Api.Wrapper.EndpointResolution;
using SevenDigital.Api.Wrapper.Exceptions;
using SevenDigital.Api.Wrapper.Utility.Http;

namespace SevenDigital.Api.Wrapper.Utility.Serialization
{
	public class ResponseParser<T> : IResponseParser<T> where T : class
	{
		private readonly IApiResponseDetector _apiResponseDetector;

		public ResponseParser()
			: this(EssentialDependencyCheck<IApiResponseDetector>.Instance)
		{
		}

		public ResponseParser(IApiResponseDetector apiResponseDetector)
		{
			_apiResponseDetector = apiResponseDetector;
		}

		public T Parse(Response response)
		{
			CheckResponse(response);
			return ParseResponse(response);
		}

		private void CheckResponse(Response response)
		{
			if (response == null)
			{
				throw new ArgumentNullException("response");
			}

			if (string.IsNullOrEmpty(response.Body))
			{
				throw ExceptionFactory.CreateNonXmlResponseException(response);
			}

			var messageIsXml = _apiResponseDetector.IsXml(response.Body);

			if (messageIsXml && _apiResponseDetector.IsApiErrorResponse(response.Body))
			{
				var error = ParseError(response);
				throw ExceptionFactory.CreateApiErrorException(error, response);
			}

			if (_apiResponseDetector.IsServerError((int)response.StatusCode))
			{
				if (!messageIsXml)
				{
					throw ExceptionFactory.CreateNonXmlResponseException(response);
				}

				var error = ParseError(response);
				throw ExceptionFactory.CreateApiErrorException(error, response);
			}

			if (!messageIsXml && response.StatusCode != HttpStatusCode.OK)
			{
				throw ExceptionFactory.CreateNonXmlResponseException(response);
			}

			if (messageIsXml && !_apiResponseDetector.IsApiOkResponse(response.Body))
			{
				throw ExceptionFactory.CreateUnrecognisedStatusException(response);
			}
		}

		private Error ParseError(Response response)
		{
			try
			{
				var xmlDoc = XDocument.Parse(response.Body);
				var responseNode = xmlDoc.FirstNode as XElement;
				var errorNode = responseNode.FirstNode as XElement;
				var errorMessage = errorNode.FirstNode as XElement;
				var errorCode = ParseErrorCode(errorNode);

				return new Error
					{
						Code = errorCode,
						ErrorMessage = errorMessage.Value
					};
			}
			catch(Exception ex)
			{
				throw ExceptionFactory.CreateUnrecognisedErrorException(response, ex);
			}
		}

		private int ParseErrorCode(XElement errorNode)
		{
			var attribute = errorNode.Attribute("code");
			return int.Parse(attribute.Value);
		}

		private static T ParseResponse(Response response)
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