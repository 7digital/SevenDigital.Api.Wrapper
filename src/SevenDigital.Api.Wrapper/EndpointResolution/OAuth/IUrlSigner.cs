using System;

namespace SevenDigital.Api.Wrapper.EndpointResolution.OAuth
{
	public interface IUrlSigner {
		Uri SignUrl(string urlWithParameters, string userToken, string userSecret, IOAuthCredentials consumerCredentials);
	}
}