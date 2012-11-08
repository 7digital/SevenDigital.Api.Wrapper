using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Text;
using SevenDigital.Api.Wrapper.EndpointResolution;

namespace SevenDigital.Api.Wrapper.Utility.Http
{
	public class GzipHttpClient : IHttpClient
	{
		public Response Get(GetRequest request)
		{
			var webRequest = MakeWebRequest(request.Url, "GET", request.Headers);
			
			return TryGetResponse(webRequest.GetResponse);
		}

		public void GetAsync(GetRequest request, Action<Response> callback)
		{
			var webRequest = MakeWebRequest(request.Url, "GET", request.Headers);
			webRequest.BeginGetResponse(iar => callback(GetAsyncResponse(iar)), webRequest);
		}

		public Response Post(PostRequest request)
		{
			var webRequest = MakePostRequest(request);
			
			return TryGetResponse(webRequest.GetResponse);
		}

		public void PostAsync(PostRequest request, Action<Response> callback)
		{
			var webRequest = MakePostRequest(request);

			webRequest.BeginGetResponse(iar => callback(GetAsyncResponse(iar)), webRequest);
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

		private Response GetAsyncResponse(IAsyncResult iar)
		{
			var webRequest = (WebRequest)iar.AsyncState;

			return TryGetResponse(() => webRequest.EndGetResponse(iar));
		}

		private Response TryGetResponse(Func<WebResponse> getResponse)
		{
			WebResponse webResponse;
			try
			{
				webResponse = getResponse();
			}
			catch (WebException ex)
			{
				if (ex.Response == null)
				{
					throw;
				}
				webResponse = ex.Response;
			}

			return MakeResponse(webResponse);
		}

		private static HttpWebRequest MakeWebRequest(string url, string method, IEnumerable<KeyValuePair<string, string>> headers)
		{
			HttpWebRequest httpWebRequest;
			try {
				var uri = new Uri(url);
				var webRequest = WebRequest.Create(uri);
				httpWebRequest =  (HttpWebRequest)webRequest;
			} catch (Exception ex) {
				throw new InvalidOperationException("Could not create HttpWebRequest for url " + url, ex);
			}

			httpWebRequest.Method = method;
			httpWebRequest.UserAgent = "7digital .Net Api Wrapper";
			httpWebRequest.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip");

			foreach (var header in headers)
			{
				httpWebRequest.Headers.Add(header.Key, header.Value);
			}
			return httpWebRequest;
		}

		private Response MakeResponse(WebResponse webResponse)
		{
			string output;
			using (var sr = new StreamReader(GetResponseStream(webResponse)))
			{
				output = sr.ReadToEnd();
			}

			var statusCode = ReadStatusCode(webResponse);
			var headers = MapHeaders(webResponse.Headers);

			var response = new Response(statusCode, headers, output);

			webResponse.Close();

			return response;
		}

		private Stream GetResponseStream(WebResponse webResponse)
		{
			string contentEncodingHeader = webResponse.Headers["Content-Encoding"];

			var responseStream = webResponse.GetResponseStream();

			if (contentEncodingHeader != null && contentEncodingHeader == "gzip")
				responseStream = new GZipStream(webResponse.GetResponseStream(), CompressionMode.Decompress);

			return responseStream;
		}

		private static HttpWebRequest MakePostRequest(PostRequest request)
		{
			var webRequest = MakeWebRequest(request.Url, "POST", request.Headers);
			webRequest.ContentType = "application/x-www-form-urlencoded";

			var postData = request.Parameters.ToQueryString();
			var postBytes = Encoding.UTF8.GetBytes(postData);
			webRequest.ContentLength = postBytes.Length;

			using (Stream dataStream = webRequest.GetRequestStream())
			{
				dataStream.Write(postBytes, 0, postBytes.Length);
			}
			return webRequest;
		}

		private static HttpStatusCode ReadStatusCode(WebResponse webResponse)
		{
			var httpResponse = webResponse as HttpWebResponse;
			if (httpResponse == null)
			{
				return HttpStatusCode.NoContent;
			}

			return httpResponse.StatusCode;
		}
	}
}