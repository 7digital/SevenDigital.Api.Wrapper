using System;
using System.Xml;
using System.Xml.Linq;

using SevenDigital.Api.Wrapper.Exceptions;
using SevenDigital.Api.Wrapper.Http;

namespace SevenDigital.Api.Wrapper.Serialization
{
	public class ApiResponseDetector : IApiResponseDetector
	{
		private string StartOfMessage(string responseBody)
		{
			if (string.IsNullOrEmpty(responseBody))
			{
				return string.Empty;
			}

			var maxLength = Math.Min(responseBody.Length, 512);
			return responseBody.Substring(0, maxLength);
		}

		public bool IsXml(string responseBody)
		{
			return responseBody.StartsWith("<?xml");
		}

		public void TestXmlParse(Response response)
		{
			try
			{
				XDocument.Parse(response.Body);
			}
			catch (XmlException xmlEx)
			{
				throw new NonXmlResponseException(xmlEx, response);
			}
		}

		public bool IsApiOkResponse(string responseBody)
		{
			var startOfBody = StartOfMessage(responseBody);
			return startOfBody.Contains("<response") && startOfBody.Contains("status=\"ok\"");
		}

		public bool IsApiErrorResponse(string responseBody)
		{
			var startOfBody = StartOfMessage(responseBody);
			return startOfBody.Contains("<response") && startOfBody.Contains("status=\"error\"");
		}

		public bool IsServerError(int httpStatusCode)
		{
			return httpStatusCode >= 500;
		}
         
		public bool IsOAuthError(string responseBody)
		{
			return responseBody.StartsWith("OAuth");
		}
	}
}