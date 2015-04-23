using System;
using System.Net.Http;

namespace SevenDigital.Api.Wrapper.Responses
{
	public class CacheHeaderReader
	{
		private const string CacheControlKey = "cache-control";
		private const string MaxAgePrefix = "max-age=";

		public DateTimeOffset? GetExpiration(Response response)
		{
			if (!IsCachableResponse(response))
			{
				return null;
			}

			var headerValue = response.Headers[CacheControlKey];
			if (!IsCachableHeaderValue(headerValue))
			{
				return null;
			}

			var cacheDuration = CacheControlHeaderValue(headerValue);
			if (cacheDuration == 0)
			{
				return null;
			}

			return DateTimeOffset.UtcNow.AddSeconds(cacheDuration);
		}

		private bool IsCachableResponse(Response response)
		{
			return (response.OriginalRequest.Method == HttpMethod.Get) &&
				(response.Headers.ContainsKey(CacheControlKey));
		}

		private bool IsCachableHeaderValue(string headerValue)
		{
			if (string.IsNullOrWhiteSpace(headerValue) ||
				headerValue.Contains("no-cache") ||
				headerValue.Contains("no-store"))
			{
				return false;
			}

			if (!headerValue.Contains(MaxAgePrefix))
			{
				return false;
			}

			return true;
		}

		private int CacheControlHeaderValue(string headerValue)
		{
			var ageString = ExtractMaxAgeStringValue(headerValue);

			int ageValue;
			var parsed = int.TryParse(ageString, out ageValue);
			return parsed ? ageValue : 0;
		}

		private static string ExtractMaxAgeStringValue(string cacheControlValue)
		{
			var ageIndex = cacheControlValue.IndexOf(MaxAgePrefix, StringComparison.OrdinalIgnoreCase);
			var maxAgeValueText = cacheControlValue.Substring(ageIndex + MaxAgePrefix.Length);
			maxAgeValueText = maxAgeValueText.TrimStart();

			var trailingSpaceIndex = maxAgeValueText.IndexOf(" ", StringComparison.OrdinalIgnoreCase);
			if (trailingSpaceIndex > 0)
			{
				maxAgeValueText = maxAgeValueText.Substring(0, trailingSpaceIndex);
			}

			return maxAgeValueText;
		}
	}
}