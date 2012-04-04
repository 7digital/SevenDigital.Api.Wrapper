using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace SevenDigital.Api.Wrapper.Utility.Http
{
	public interface IHttpClient
	{
		IResponse<string> Get(IRequest request);
		void GetAsync(IRequest request, Action<IResponse<string>> callback);

		IResponse<string> Post(IRequest request, string data);
		void PostAsync(IRequest request, string data, Action<IResponse<string>> callback);
	}

	public class HttpClient : IHttpClient
	{
		public IResponse<string> Get(IRequest request)
		{
			var webRequest = (HttpWebRequest)WebRequest.Create(request.Url);
			webRequest.Method = "GET";
			webRequest.UserAgent = "7digital .Net Api Wrapper";

			foreach (var header in request.Headers)
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
			
			var output = string.Empty;
			using (var sr = new StreamReader(webResponse.GetResponseStream()))
			{
				output = sr.ReadToEnd();
			}

			var response = new Response<string> 
			{
				Body = output,
				Headers = MapHeaders(webResponse.Headers)
			};

			return response;
		}

		public void GetAsync(IRequest request, Action<IResponse<string>> callback)
		{
			var client = new WebClient();
			client.DownloadDataCompleted += (s, e) =>
												{
													var response = new Response<string>()
																	{
																		Body = System.Text.Encoding.UTF8.GetString(e.Result),
																		Headers = MapHeaders(client.ResponseHeaders)
																	};
													callback(response);
												};
			client.DownloadDataAsync(new Uri(request.Url));
		}

		public IResponse<string> Post(IRequest request, string data)
		{
			throw new NotImplementedException();
		}

		public void PostAsync(IRequest request, string data, Action<IResponse<string>> callback)
		{
			throw new NotImplementedException();
		}

		public Dictionary<string, string> MapHeaders(WebHeaderCollection headerCollection)
		{
			var headers = new Dictionary<string, string>();

			for (var i = 0; i < headerCollection.Count; i++)
			{
				headers.Add(headerCollection.GetKey(i), string.Join(",", headerCollection.GetValues(i)));
			}

			return headers;
		}
	}

	public interface IRequest
	{
		string Url { get; }
		Dictionary<string, string> Headers { get; } 
	}

	public class Request : IRequest
	{
		private readonly string _url;
		private readonly Dictionary<string, string> _headers;

		public Request(string url, Dictionary<string, string> headers)
		{
			_url = url;
			_headers = headers;
		}

		public string Url
		{
			get { return _url; }
		}

		public Dictionary<string, string> Headers
		{
			get { return _headers; }
		}
	}
}