using System;
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
		private readonly IOAuthCredentials _oAuthCredentials;
		private string _apiUrl = "http://api.7digital.com/1.2";

	    public EndpointResolver(IUrlResolver urlResolver, IUrlSigner urlSigner, IOAuthCredentials oAuthCredentials)
		{
	    	_urlResolver = urlResolver;
	    	_urlSigner = urlSigner;
	    	_oAuthCredentials = oAuthCredentials;
		}

	    public XmlNode HitEndpoint(EndPointInfo endPointInfo)
		{
			string output = GetEndpointOutput(endPointInfo);
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
			string statusAttribute = response.Attributes["status"] == null ? response.Name : response.Attributes["status"].Value;
			if (statusAttribute == "error")
				throw new ApiXmlException("An error has occured in the Api, see Error property for details", response.FirstChild);
		}

		private static XmlNode GetResponseNode(string output)
		{
			var xml = new XmlDocument();
			try
			{
				xml.LoadXml(output);
			} 
			catch(XmlException)
			{
				string errorXml = string.Format("<?xml version=\"1.0\" encoding=\"utf-8\" ?><response status=\"error\" version=\"1.2\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:noNamespaceSchemaLocation=\"http://api.7digital.com/1.2/static/7digitalAPI.xsd\" ><error code=\"9001\"><errorMessage>{0}</errorMessage></error></response>", output);
				xml.LoadXml(errorXml);
			}
			return xml.SelectSingleNode("/response");
		}

		private string GetEndpointOutput(EndPointInfo endPointInfo)
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

			return _urlResolver.Resolve(signedUrl, endPointInfo.HttpMethod, new WebHeaderCollection());
		}
	}
}