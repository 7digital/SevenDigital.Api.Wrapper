using System;
using System.Net;
using System.Xml;
using SevenDigital.Api.Wrapper.EndpointResolution.OAuth;
using SevenDigital.Api.Wrapper.Exceptions;
using SevenDigital.Api.Wrapper.Utility.Http;
using System.Collections.Generic;

namespace SevenDigital.Api.Wrapper.EndpointResolution
{
	public class EndpointResolver : IEndpointResolver
	{
		private readonly IUrlResolver _urlResolver;
		private readonly IUrlSigner _urlSigner;
		private readonly IOAuthCredentials _oAuthCredentials;
		private string _apiUrl = "http://api.7digital.com/1.2";

	    public EndpointResolver(IUrlResolver urlResolver, IUrlSigner urlSigner, IOAuthCredentials oAuthCredentials)
		{
	    	_urlResolver = urlResolver;
	    	_urlSigner = urlSigner;
	    	_oAuthCredentials = oAuthCredentials;
		}

	    public string HitEndpoint(EndPointInfo endPointInfo)
		{
	        Uri signedUrl = GetSignedUrl(endPointInfo);

	        return _urlResolver.Resolve(signedUrl, endPointInfo.HttpMethod, new Dictionary<string,string>());
		}

        public void HitEndpointAsync(EndPointInfo endPointInfo,Action<string> payload)
        {
            Uri signedUrl = GetSignedUrl(endPointInfo);

            var result = _urlResolver.Resolve(signedUrl, endPointInfo.HttpMethod, new Dictionary<string, string>());

            payload.BeginInvoke(result, null, null);
        }
	    private Uri GetSignedUrl(EndPointInfo endPointInfo)
	    {
	        if (endPointInfo.UseHttps)
	            _apiUrl = _apiUrl.Replace("http://", "https://");

	        var uriString = string.Format("{0}/{1}?oauth_consumer_key={2}&{3}", 
	                                      _apiUrl, endPointInfo.Uri,
	                                      _oAuthCredentials.ConsumerKey, 
	                                      endPointInfo.Parameters.ToQueryString()).TrimEnd('&');

	        var signedUrl = new Uri(uriString);
 
	        if(endPointInfo.IsSigned)
	            signedUrl = _urlSigner.SignUrl(uriString, endPointInfo.UserToken, endPointInfo.UserSecret,_oAuthCredentials);
	        return signedUrl;
	    }
	}
}