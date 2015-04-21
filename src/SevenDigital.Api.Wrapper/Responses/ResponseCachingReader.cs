using System;
using System.Net.Http;

namespace SevenDigital.Api.Wrapper.Responses
{
	public static class ResponseCachingReader
	{
		private const string CacheControlKey = "cache-control";

		public static bool IsCachable(Response response)
		{
			if (response.OriginalRequest.Method != HttpMethod.Get)
			{
				return false;
			}

			if (!response.Headers.ContainsKey(CacheControlKey))
			{
				return false;
			}

			return true;
		}

		public static int DurationSeconds(Response response)
		{
			if (!IsCachable(response))
			{
				return 0;
			}

			var cacheControlValue = response.Headers[CacheControlKey];
			return CacheControlHeaderValue(cacheControlValue);
		}

		private static int CacheControlHeaderValue(string headerValue)
		{
			if (string.IsNullOrWhiteSpace(headerValue) ||
				headerValue.Contains("no-cache") ||
				headerValue.Contains("no-store"))
			{
				return 0;
			}

			if (!headerValue.Contains("max-age"))
			{
				return 0;
			}

			var ageString = ExtractMaxAgeStringValue(headerValue);
			if (string.IsNullOrWhiteSpace(ageString))
			{
				return 0;
			}

			int ageValue;
			var parsed = int.TryParse(ageString, out ageValue);
			if (parsed)
			{
				return ageValue;
			}

			return 0;
		}

		private static string ExtractMaxAgeStringValue(string cacheControlValue)
		{
			const string MaxAgePrefix = "max-age:";
			var ageIndex = cacheControlValue.IndexOf(MaxAgePrefix, StringComparison.OrdinalIgnoreCase);
			var ageString = cacheControlValue.Substring(ageIndex + MaxAgePrefix.Length);
			ageString = ageString.TrimStart();

			var spaceIndex = ageString.IndexOf(" ", StringComparison.OrdinalIgnoreCase);
			if (spaceIndex > 0)
			{
				ageString = ageString.Substring(0, spaceIndex);
			}

			return ageString;
		}
	}
}