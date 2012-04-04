using System;
using System.Collections.Generic;
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
			throw new NotSupportedException("Need to use async in windows mobile");

		}

		public void GetAsync(IRequest request, Action<IResponse<string>> callback)
		{
			var client = new WebClient();
			client.DownloadStringCompleted += (s, e) =>
			{
				var response = new Response<string>()
				{
					Body = e.Result,
					Headers = MapHeaders(client.ResponseHeaders)
				};
				callback(response);
			};

			client.DownloadStringAsync(new Uri(request.Url));
		}

		public IResponse<string> Post(IRequest request, string data)
		{
			throw new NotSupportedException("Need to use async in windows mobile");
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
				headers.Add(headerCollection.AllKeys[i], string.Join(",", headerCollection[headerCollection.AllKeys[i]]));
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