using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace SevenDigital.Api.Wrapper.Utility.Http
{
    public class HttpGetResolver : IUrlResolver
    {
        public string Resolve(Uri endpoint, string method, Dictionary<string,string> headers)
        {
            var webRequest = (HttpWebRequest)WebRequest.Create(endpoint.OriginalString);
            webRequest.Method = method;
            
            webRequest.Headers = new WebHeaderCollection();    
            
            
            IAsyncResult r = (IAsyncResult)webRequest.BeginGetRequestStream(null, null);

            Stream postStream = webRequest.EndGetRequestStream(r);

            return new StreamReader(postStream).ReadToEnd();
        }
    }
}
