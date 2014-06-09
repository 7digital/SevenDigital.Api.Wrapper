using System;
using System.Xml.Linq;
using SevenDigital.Api.Schema;
using SevenDigital.Api.Wrapper.Exceptions;
using SevenDigital.Api.Wrapper.Responses.Parsing.Exceptions;

namespace SevenDigital.Api.Wrapper.Responses.Parsing
{
	public class ResponseParser: IResponseParser
	{
		private readonly IApiResponseDetector _apiResponseDetector;
		
		public ResponseParser(IApiResponseDetector apiResponseDetector)
		{
			_apiResponseDetector = apiResponseDetector;
		}

		public T Parse<T>(Response response) where T : class, new()
		{
			DetectErrorResponsesAndThrow(response);
			return ParseResponse<T>(response);
		}

		private void DetectErrorResponsesAndThrow(Response response)
		{
			if (response == null)
			{
				throw new ArgumentNullException("response");
			}

			if (string.IsNullOrEmpty(response.Body))
			{
				throw new NonXmlResponseException(response);
			}

			if (!_apiResponseDetector.IsXml(response.Body))
			{
				DetectAndThrowForNonXmlResponses(response);
			}

			if (!_apiResponseDetector.IsApiOkResponse(response.Body) && !_apiResponseDetector.IsApiErrorResponse(response.Body))
			{
				throw new UnrecognisedStatusException(response);
			}

			if (_apiResponseDetector.IsApiOkResponse(response.Body) && !_apiResponseDetector.IsApiErrorResponse(response.Body))
			{
				return;
			}

			var error = ParseError(response);
			throw ExceptionFactory.CreateApiErrorException(error, response);
		}

		private void DetectAndThrowForNonXmlResponses(Response response)
		{
			if (_apiResponseDetector.IsOAuthError(response.Body))
			{
				throw new OAuthException(response);
			}

			throw new NonXmlResponseException(response);
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
				throw new UnrecognisedErrorException(ex, response);
			}
		}

		private int ParseErrorCode(XElement errorNode)
		{
			var attribute = errorNode.Attribute("code");
			return int.Parse(attribute.Value);
		}

		private static T ParseResponse<T>(Response response) where T : class, new()
		{
			try
			{
				var deserializer = new StringDeserializer();
				return deserializer.DeserializeApiResponse<T>(response.Body);
			}
			catch (UnexpectedXmlContentException e)
			{
				throw new UnexpectedXmlResponseException(e, response);
			}
			catch (NonXmlContentException ex)
			{
				throw new NonXmlResponseException(NonXmlResponseException.DEFAULT_ERROR_MESSAGE, ex, response);
			}
		}
	}
}