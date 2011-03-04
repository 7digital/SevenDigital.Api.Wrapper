using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Net;
using System.Xml;
using SevenDigital.Api.Wrapper.Utility.Http;

namespace SevenDigital.Api.Wrapper.Repository
{
	public class EndpointResolver : IEndpointResolver
	{
		private readonly IUrlResolver _urlResolver;
		private readonly string _apiUrl = ConfigurationManager.AppSettings["Wrapper.BaseUrl"];
		private readonly string _consumerKey = ConfigurationManager.AppSettings["Wrapper.ConsumerKey"];

		public EndpointResolver(IUrlResolver urlResolver)
		{
			_urlResolver = urlResolver;
		}

		public XmlNode HitEndpoint(string endPoint, string methodName, NameValueCollection querystring)
		{
			string output = GetEndpointOutput(endPoint, methodName);
			XmlNode response = GetResponseNode(output);
			AssertError(response);

			return response.FirstChild;
		}

		private static void AssertError(XmlNode response)
		{
			string statusAttribute = response.Attributes["status"].Value;
			if (statusAttribute == "error")
				throw new ApiException("An error has occured in the Api", response.FirstChild);
		}

		private static XmlNode GetResponseNode(string output)
		{
			var xml = new XmlDocument();
			xml.LoadXml(output);
			return xml.SelectSingleNode("/response");
		}

		private string GetEndpointOutput(string endPoint, string methodName)
		{
			var endpointUri = new Uri(string.Format("{0}/{1}?oauth_consumer_key={2}", _apiUrl, endPoint, _consumerKey));
			return _urlResolver.Resolve(endpointUri, methodName, new WebHeaderCollection());
		}
	}
}