using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace SevenDigital.Api.Wrapper.Utility.Http
{
	public class HttpGetDispatcher : IRequestDispatcher
	{
		public const string METHOD = "GET";

		public string Dispatch(string endpoint, Dictionary<string, string> headers)
		{
			var webResponse = GetResponse(endpoint, METHOD, headers);
			string output;
			using (var sr = new StreamReader(webResponse.GetResponseStream()))
			{
				output = sr.ReadToEnd();
			}
			return output;
		}

		public Response<string> FullDispatch(string endpoint, Dictionary<string, string> headers)
		{
			var webResponse = GetResponse(endpoint, METHOD, headers);
			var output = string.Empty;
			
			using (var sr = new StreamReader(webResponse.GetResponseStream()))
			{
				output = sr.ReadToEnd();
			}

			var response = new Response<string>();
			response.Body = output;
			for (var i =0; i < webResponse.Headers.Count; i++)
			{
				response.Headers.Add(webResponse.Headers.GetKey(i), string.Join(",", webResponse.Headers.GetValues(i)));
			}

			return response;
		}

		private static WebResponse GetResponse(string endpoint, string method, Dictionary<string, string> headers)
		{
			var webRequest = (HttpWebRequest) WebRequest.Create(endpoint);
			webRequest.Method = method;
			webRequest.UserAgent = "7digital .Net Api Wrapper";

			foreach (var header in headers)
			{
				webRequest.Headers.Add(header.Key, header.Value);
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
			return webResponse;
		}

		public void DispatchAsync(string endpoint, Dictionary<string, string> headers, Action<string> payload)
		{
			var client = new WebClient();
			client.DownloadDataCompleted += (s, e) => payload(System.Text.Encoding.UTF8.GetString(e.Result));
			client.DownloadDataAsync(new Uri(endpoint));
		}
	}
}
