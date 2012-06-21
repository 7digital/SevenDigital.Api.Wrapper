using SevenDigital.Api.Schema.Attributes;
using SevenDigital.Api.Schema.OAuth;
using System;
using SevenDigital.Api.Schema.ParameterDefinitions.Get;

namespace SevenDigital.Api.Schema.User.Payment
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
