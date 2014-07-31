using System;
using System.Xml;

namespace SevenDigital.Api.Wrapper.Responses.Parsing
{
	public class ApiResponseDetector : IApiResponseDetector
	{
		private string StartOfMessage(string responseBody)
		{
			if (string.IsNullOrEmpty(responseBody))
			{
				return string.Empty;
			}

			var start = responseBody.IndexOf("<response", StringComparison.Ordinal);
			if (start == -1)
			{
				return string.Empty;
			}

			var end = responseBody.IndexOf(">", start, StringComparison.Ordinal);
			return responseBody.Substring(start, end - start);
		}

		public bool StartsWithXmlDeclaration(string responseBody)
		{
			return responseBody.StartsWith("<?xml");
		}

		public bool IsWellFormedXml(string responseBody)
		{
			var parser = new XmlDocument();
			try
			{
				parser.LoadXml(responseBody);
				return true;
			}
			catch (XmlException)
			{
				return false;
			}
		}

		public bool IsApiOkResponse(string responseBody)
		{
			var startOfBody = StartOfMessage(responseBody);
			return startOfBody.Contains("status=\"ok\"");
		}

		public bool IsApiErrorResponse(string responseBody)
		{
			var startOfBody = StartOfMessage(responseBody);
			return startOfBody.Contains("status=\"error\"");
		}

		public bool IsOAuthError(string responseBody)
		{
			return responseBody.StartsWith("OAuth");
		}
	}
}