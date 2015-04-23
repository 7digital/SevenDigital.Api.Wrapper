using System;
using System.Net.Http;

namespace SevenDigital.Api.Wrapper.Responses
{
	public static class ResponseCachingReader
	{
		private const string CacheControlKey = "cache-control";
		private const string MaxAgePrefix = "max-age:";

		public static bool IsCachable(Response response)
		{
			return (response.OriginalRequest.Method == HttpMethod.Get) &&
				(response.Headers.ContainsKey(CacheControlKey));
		}

		public static int DurationSeconds(Response response)
		{
			if (!IsCachable(response))
			{
				return 0;
			}

			var headerValue = response.Headers[CacheControlKey];
			return CacheControlHeaderValue(headerValue);
		}

		private static int CacheControlHeaderValue(string headerValue)
		{
			if (string.IsNullOrWhiteSpace(headerValue) ||
				headerValue.Contains("no-cache") ||
				headerValue.Contains("no-store"))
			{
				return 0;
			}

			if (!headerValue.Contains(MaxAgePrefix))
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