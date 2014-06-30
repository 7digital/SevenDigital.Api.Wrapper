using System;
using SevenDigital.Api.Schema.Attributes;
using SevenDigital.Api.Schema.OAuth;
using SevenDigital.Api.Schema.ParameterDefinitions.Get;

namespace SevenDigital.Api.Schema.Users.Payment
{
	[ApiEndpoint("user/payment/card/delete")]
	[OAuthSigned]
	[RequireSecure]
	[Serializable]
	[HttpPost]
	public class DeleteCard : HasCardIdParameter
	{
	}
}
