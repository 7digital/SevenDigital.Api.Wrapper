using System;
using System.Configuration;
using System.Net;
using System.Xml;
using SevenDigital.Api.Wrapper.Exceptions;
using SevenDigital.Api.Wrapper.Utility.Http;

namespace SevenDigital.Api.Wrapper.EndpointResolution
{
	public class EndpointResolver : IEndpointResolver
	{
		private readonly IUrlResolver _urlResolver;
		private string _apiUrl = ConfigurationManager.AppSettings["Wrapper.BaseUrl"];
		private readonly string _consumerKey = ConfigurationManager.AppSettings["Wrapper.ConsumerKey"];

		public EndpointResolver(IUrlResolver urlResolver)
		{
			_urlResolver = urlResolver;
		}

		public XmlNode HitEndpoint(EndPointState endPointState)
		{
			string output = GetEndpointOutput(endPointState);
			XmlNode response = GetResponseNode(output);
			AssertError(response);
			return response.FirstChild;
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

		private string GetEndpointOutput(EndPointState endPointState)
		{
			if (endPointState.UseHttps)
				_apiUrl = _apiUrl.Replace("http://", "https://");

			string uriString = string.Format("{0}/{1}?oauth_consumer_key={2}&{3}", 
														_apiUrl, 
														endPointState.Uri, 
														_consumerKey, 
														endPointState.Parameters.ToQueryString());

			var endpointUri = new Uri(uriString.Trim('&'));

			return _urlResolver.Resolve(endpointUri, endPointState.HttpMethod, new WebHeaderCollection());
		}
	}
}