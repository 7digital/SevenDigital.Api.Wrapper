using System;
using System.Collections.Generic;

namespace SevenDigital.Api.Wrapper.EndpointResolution.OAuth
{
	public interface IUrlSigner 
	{
		Uri SignUrl(string urlWithParameters, string userToken, string tokenSecret, IOAuthCredentials consumerCredentials);
		string SignGetUrl(string urlWithParameters, string userToken, string tokenSecret, IOAuthCredentials consumerCredentials);
		IDictionary<string, string> SignPostRequest(string url, string userToken, string tokenSecret, IOAuthCredentials consumerCredentials, IDictionary<string, string> postParameters);
	}
}