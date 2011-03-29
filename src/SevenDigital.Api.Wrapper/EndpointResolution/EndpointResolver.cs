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
		private string _apiUrl = ConfigurationManager.AppSettings["Wrapper.BaseUrl"];
	    private readonly OAuthCredentials _consumerCredentials;

	    public EndpointResolver(IUrlResolver urlResolver, OAuthCredentials consumerCredentials)
		{
		    _consumerCredentials = consumerCredentials;
		    _urlResolver = urlResolver;
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

			string uriString = string.Format("{0}/{1}?oauth_consumer_key={2}&{3}", 
														_apiUrl, 
														endPointInfo.Uri, 
														_consumerCredentials.ConsumerKey, 
														endPointInfo.Parameters.ToQueryString());

		    return GetResponse(uriString, endPointInfo.UserToken, endPointInfo.UserSecret);
		}

        private string GetResponse(string urlWithParameters, string userToken, string userSecret)
        {
            string normalizedRequestParameters;
            string normalizedUrl;

            OAuthBase oAuthBase = new OAuthBase();
            var timestamp = oAuthBase.GenerateTimeStamp();
            var nonce = oAuthBase.GenerateNonce();
            var signature = oAuthBase.GenerateSignature(new Uri(urlWithParameters), _consumerCredentials.ConsumerKey,
                                                        _consumerCredentials.ConsumerSecret,
                                                        userToken, userSecret, "GET", timestamp, nonce,
                                                        out normalizedUrl,
                                                        out normalizedRequestParameters,
                                                        new Dictionary<string, string>());

            var signedUrl = string.Format("{0}?{1}&oauth_signature={2}", normalizedUrl, normalizedRequestParameters,
                                          signature);

            var request = HttpWebRequest.Create(signedUrl);

            var webResponse = request.GetResponse();
            using (var responseStream = new StreamReader(webResponse.GetResponseStream()))
            {
                return responseStream.ReadToEnd();
            }

        }
	}

    public class OAuthCredentials
    {
        protected OAuthCredentials() { }

        public OAuthCredentials(string consumerKey, string consumerSecret)
        {
            ConsumerKey = consumerKey;
            ConsumerSecret = consumerSecret;
        }

        public string ConsumerKey { get; protected set; }
        public string ConsumerSecret { get; protected set; }
    }
}