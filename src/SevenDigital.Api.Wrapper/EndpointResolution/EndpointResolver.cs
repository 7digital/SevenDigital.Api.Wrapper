using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Xml;
using SevenDigital.Api.Wrapper.EndpointResolution.OAuth;
using SevenDigital.Api.Wrapper.Exceptions;
using SevenDigital.Api.Wrapper.Utility.Http;

namespace SevenDigital.Api.Wrapper.EndpointResolution
{
	public class EndpointResolver : IEndpointResolver
	{
		private readonly IUrlResolver _urlResolver;
		private readonly IUrlSigner _urlSigner;
		private string _apiUrl = ConfigurationManager.AppSettings["Wrapper.BaseUrl"];
	    private readonly OAuthCredentials _consumerCredentials;

	    public EndpointResolver(IUrlResolver urlResolver, IUrlSigner urlSigner, OAuthCredentials consumerCredentials)
		{
	    	_urlResolver = urlResolver;
	    	_urlSigner = urlSigner;
	    	_consumerCredentials = consumerCredentials;
		}

	    public XmlNode HitEndpoint(EndPointInfo endPointInfo)
		{
			string output = GetEndpointOutput(endPointInfo);
	        Debug.WriteLine(output);
			XmlNode response = GetResponseNode(output);
			AssertError(response);
			return response.FirstChild;
		}

		public string GetRawXml(EndPointInfo endPointInfo)
		{
			return GetEndpointOutput(endPointInfo);
		}

		private static void AssertError(XmlNode response)
		{
			string statusAttribute = response.Attributes["status"].Value;
			if (statusAttribute == "error")
				throw new ApiXmlException("An error has occured in the Api, see Error property for details", response.FirstChild);
		}

		private static XmlNode GetResponseNode(string output)
		{
			var xml = new XmlDocument();
			xml.LoadXml(output);
			return xml.SelectSingleNode("/response");
		}

		private string GetEndpointOutput(EndPointInfo endPointInfo)
		{
			if (endPointInfo.UseHttps)
				_apiUrl = _apiUrl.Replace("http://", "https://");

			var uriString = string.Format("{0}/{1}?oauth_consumer_key={2}&{3}", _apiUrl, endPointInfo.Uri, 
				_consumerCredentials.ConsumerKey, endPointInfo.Parameters.ToQueryString());

			var signedUrl = _urlSigner.SignUrl(uriString, endPointInfo.UserToken, endPointInfo.UserSecret, _consumerCredentials);

			return _urlResolver.Resolve(signedUrl, endPointInfo.HttpMethod, new WebHeaderCollection());
		}
	}
}