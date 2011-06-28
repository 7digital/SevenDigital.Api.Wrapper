using System;
using System.Collections.Generic;

namespace SevenDigital.Api.Wrapper.EndpointResolution.OAuth
{
	public interface IUrlSigner {
		Uri SignUrl(string urlWithParameters, string userToken, string userSecret, IOAuthCredentials consumerCredentials);
		string SignUrlAsString(string urlWithParameters, string userToken, string userSecret, IOAuthCredentials consumerCredentials);
	    IDictionary<string, string> SignPostRequest(string url, string userToken, string userSecret, IOAuthCredentials consumerCredentials, Dictionary<string, string> postParameters);
	}
}