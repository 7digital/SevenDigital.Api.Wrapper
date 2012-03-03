using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace SevenDigital.Api.Wrapper.Utility.Http
{
    public class HttpGetResolver : IUrlResolver
    {
        public string Resolve(string endpoint, string method, Dictionary<string,string> headers)
        {
            var webRequest = (HttpWebRequest)WebRequest.Create(endpoint);
            webRequest.Method = method;
			webRequest.UserAgent = "7digital .Net Api Wrapper";

            foreach (var header in headers)
            {
                webRequest.Headers.Add(header.Key,header.Value);    
            }
            
            WebResponse webResponse;
            try
            {
                webResponse = webRequest.GetResponse();
            }
            catch (WebException ex)
            {
                webResponse = ex.Response;
            }
            string output;
            using (var sr = new StreamReader(webResponse.GetResponseStream()))
            {
                output = sr.ReadToEnd();
            }
            return output;
        }

        public void ResolveAsync(string endpoint, string method, Dictionary<string, string> headers, Action<string> payload)
        {
            var client = new WebClient();
            client.DownloadDataCompleted += (s, e) => payload(System.Text.Encoding.UTF8.GetString(e.Result));
            client.DownloadDataAsync(new Uri(endpoint));
        }
    }
}
