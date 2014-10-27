namespace SevenDigital.Api.Wrapper.Requests
{
	public static class UriPath
	{
		public static string Combine(string baseUri, string rest)
		{
			if (string.IsNullOrEmpty(baseUri))
			{
				return rest;
			}

			if (string.IsNullOrEmpty(rest))
			{
				return baseUri;
			}

			bool baseHasTrailingSlash = baseUri.EndsWith("/");
			bool restHasLeadingSlash = rest.StartsWith("/");

			if (baseHasTrailingSlash && restHasLeadingSlash)
			{
				return baseUri.Substring(0, baseUri.Length - 1) + rest;
			}

			if (baseHasTrailingSlash || restHasLeadingSlash)
			{
				return baseUri + rest;
			}

			return baseUri + "/" + rest;
		}
	}
}
