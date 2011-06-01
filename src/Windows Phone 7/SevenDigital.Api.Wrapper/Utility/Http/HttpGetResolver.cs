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
           throw new NotSupportedException("Need to use async in windows mobile");
        }

        public void ResolveAsync(Uri endpoint, string method, Dictionary<string, string> headers, Action<string> payload)
        {
            var client = new WebClient();
            client.DownloadStringCompleted += (s, e) => payload(e.Result);
            client.DownloadStringAsync(endpoint);
        }
    }
}
