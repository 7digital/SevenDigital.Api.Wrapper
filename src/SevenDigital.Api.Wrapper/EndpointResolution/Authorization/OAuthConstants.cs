namespace SevenDigital.Api.Wrapper.EndpointResolution.Authorization
{
	public class OAuthConstants
	{
		public const string O_AUTH_VERSION = "1.0";
		public const string O_AUTH_PARAMETER_PREFIX = "oauth_";
		public const string OAUTH_CONSUMER_KEY_KEY = "oauth_consumer_key";
		public const string O_AUTH_CALLBACK_KEY = "oauth_callback";
		public const string O_AUTH_VERSION_KEY = "oauth_version";
		public const string O_AUTH_SIGNATURE_METHOD_KEY = "oauth_signature_method";
		public const string O_AUTH_SIGNATURE_KEY = "oauth_signature";
		public const string O_AUTH_TIMESTAMP_KEY = "oauth_timestamp";
		public const string O_AUTH_NONCE_KEY = "oauth_nonce";
		public const string O_AUTH_TOKEN_KEY = "oauth_token";
		public const string O_AUTH_TOKEN_SECRET_KEY = "oauth_token_secret";
		public const string HMACSHA1_SIGNATURE_TYPE = "HMAC-SHA1";
		public const string PLAIN_TEXT_SIGNATURE_TYPE = "PLAINTEXT";
		public const string RSASHA1_SIGNATURE_TYPE = "RSA-SHA1";
		public const string UNRESERVED_CHARS = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_.~";
	}
}