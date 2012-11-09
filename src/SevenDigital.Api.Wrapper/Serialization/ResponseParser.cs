﻿using System;
using System.Xml.Linq;
using SevenDigital.Api.Schema;
using SevenDigital.Api.Wrapper.EndpointResolution;
using SevenDigital.Api.Wrapper.Exceptions;
using SevenDigital.Api.Wrapper.Http;

namespace SevenDigital.Api.Wrapper.Serialization
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
			DetectErrorResponsesAndThrow(response);
			return ParseResponse(response);
		}

		private void DetectErrorResponsesAndThrow(Response response)
		{
			if (response == null)
				throw new ArgumentNullException("response");

			if (string.IsNullOrEmpty(response.Body))
				throw new NonXmlResponseException(response);

			if (!_apiResponseDetector.IsXml(response.Body))
				DetectAndThrowForNonXmlResponses(response);

			if (!_apiResponseDetector.IsApiOkResponse(response.Body) && !_apiResponseDetector.IsApiErrorResponse(response.Body))
				throw new UnrecognisedStatusException(response);

			if (_apiResponseDetector.IsApiOkResponse(response.Body) && !_apiResponseDetector.IsApiErrorResponse(response.Body))
				return;

			var error = ParseError(response);
			throw ExceptionFactory.CreateApiErrorException(error, response);
		}

		private void DetectAndThrowForNonXmlResponses(Response response)
		{
			if (_apiResponseDetector.IsOAuthError(response.Body))
				throw new OAuthException(response);

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

		private static T ParseResponse(Response response)
		{
			try
			{
				var deserializer = new StringDeserializer<T>();
				return deserializer.Deserialize(response.Body);
			}
			catch (Exception ex)
			{
				throw new NonXmlResponseException(NonXmlResponseException.DEFAULT_ERROR_MESSAGE, ex, response);
			}
		}
	}

}