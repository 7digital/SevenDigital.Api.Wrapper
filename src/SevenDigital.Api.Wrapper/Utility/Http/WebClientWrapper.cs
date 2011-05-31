using System.Collections.Generic;
using System.Net;
using System.Text;

namespace SevenDigital.Api.Wrapper.Utility.Http
{
	public class WebClientWrapper : IWebClientWrapper {
		private readonly WebClient _client;

		public WebClientWrapper(WebClient client) {
			_client = client;
		}

		public string UploadString(string address, string method, string data) {
			return _client.UploadString(address, method, data);
		}

		public Encoding Encoding {
			get { return _client.Encoding; }
			set { _client.Encoding = value; }
		}

		public WebHeaderCollection Headers {
			get { return _client.Headers ?? new WebHeaderCollection(); }
			set { _client.Headers = value; }
		}

		public void Dispose() {
			_client.Dispose();
		}
	}
}