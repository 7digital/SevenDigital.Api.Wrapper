namespace SevenDigital.Api.Wrapper.Schema.OAuth
{
	public static class OauthAccessTokenExtensions
	{
		public static IFluentApi<OathAccessToken> WithOAuthRequestToken(this IFluentApi<OathAccessToken> api, string oAuthRequestToken) {
			api.WithParameter("oauth_token", oAuthRequestToken);
			return api;
		} 
	}
}