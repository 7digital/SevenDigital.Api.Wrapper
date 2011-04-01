using System;
using System.IO;
using System.Net;

namespace SevenDigital.Api.Wrapper.Utility.Http {
	public class HttpGetResolver : IUrlResolver  {

		public string Resolve(Uri endpoint, string method, WebHeaderCollection headers) {
			var webRequest = (HttpWebRequest)WebRequest.Create(endpoint.ToString());
			webRequest.Method = method;
			webRequest.Headers.Add(headers);
			using (var webResponse = webRequest.GetResponse()) {
				string output;
				using (var sr = new StreamReader(webResponse.GetResponseStream())) {
					output = sr.ReadToEnd();
				}
				return output;
			}
		}
	}
}
