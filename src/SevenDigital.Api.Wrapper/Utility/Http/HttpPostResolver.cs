using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace SevenDigital.Api.Wrapper.Utility.Http
{
    public class HttpPostResolver : IUrlResolver
    {
        private readonly IWebClientFactory _webClientFactory;
        private string _parametersAsString = string.Empty;

        public string ParametersAsString
        {
            get { return _parametersAsString; }
            set { _parametersAsString = value; }
        }

        public HttpPostResolver(IWebClientFactory webClientFactory)
        {
            _webClientFactory = webClientFactory;
        }
        public HttpPostResolver(IWebClientFactory webClientFactory, string parametersAsString)
        {
            _webClientFactory = webClientFactory;
            ParametersAsString = parametersAsString;
        }

        public string Resolve(string endpoint, string method, Dictionary<string, string> headers)
        {
            using (var webClientWrapper = _webClientFactory.GetWebClient())
            {
                webClientWrapper.Encoding = Encoding.UTF8;
                webClientWrapper.Headers = new WebHeaderCollection();
				webClientWrapper.Headers[HttpRequestHeader.UserAgent] = "7digital .Net Api Wrapper";

                return webClientWrapper.UploadString(endpoint, method, ParametersAsString);
            }
        }

        public void ResolveAsync(string endpoint, string method, Dictionary<string, string> headers, Action<string> payload)
        {
			throw new NotImplementedException();
			//var client = new WebClient();
			//client.UploadDataCompleted += (s, e) => payload(Encoding.UTF8.GetString(e.Result));
			//client.UploadDataAsync(endpoint, Encoding.UTF8.GetBytes(ParametersAsString));
        }
    }
}