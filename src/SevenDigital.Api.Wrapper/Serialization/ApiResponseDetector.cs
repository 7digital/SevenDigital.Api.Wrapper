using System;

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
			return responseBody.Trim().StartsWith("<?xml");
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

		public bool IsOAuthError(string responseBody)
		{
			return responseBody.StartsWith("OAuth");
		}
	}
}