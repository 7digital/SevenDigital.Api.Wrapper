using SevenDigital.Api.Schema.Attributes;
using SevenDigital.Api.Schema.OAuth;

namespace SevenDigital.Api.Schema.CustomerSupport
{
	[ApiEndpoint("customersupport")]
	[OAuthSigned]
	[RequireSecure]
	[HttpPost]
	public class CustomerSupport
	{
	}
}
