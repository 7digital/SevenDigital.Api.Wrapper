using SevenDigital.Api.Schema.Attributes;
using SevenDigital.Api.Schema.OAuth;
using System;

namespace SevenDigital.Api.Schema.User.Payment
{
	[ApiEndpoint("user/payment/card/select")]
	[OAuthSigned]
	[RequireSecure]
	[Serializable]
	[HttpPost]
	public class DefaultCard
	{
	}
}
