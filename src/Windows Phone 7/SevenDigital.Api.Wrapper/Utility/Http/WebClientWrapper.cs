using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;

namespace SevenDigital.Api.Wrapper.Utility.Http
{
    public class WebClientWrapper : IWebClientWrapper
    {
        private readonly WebClient _client;

        public WebClientWrapper(WebClient client)
        {
            _client = client;
        }


        public string UploadString(string address, string method, string data)
        {

            string result = string.Empty;
            AutoResetEvent autoReset = new AutoResetEvent(false);

            _client.UploadStringCompleted += (e, a) =>
            {
                result = a.Result;
                autoReset.Set();
            };
            _client.UploadStringAsync(new System.Uri(address), method, data);
            autoReset.WaitOne();

            return result;
        }

        public Encoding Encoding
        {
            get { return _client.Encoding; }
            set { _client.Encoding = value; }
        }

        public WebHeaderCollection Headers
        {
            get { return _client.Headers ?? new WebHeaderCollection(); }
            set { _client.Headers = value; }
        }

        public void Dispose()
        {
            
        }
    }
}