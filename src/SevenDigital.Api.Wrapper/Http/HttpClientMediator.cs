using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using SevenDigital.Api.Wrapper.Requests;
using SevenDigital.Api.Wrapper.Responses;

namespace SevenDigital.Api.Wrapper.Http
{
	public class HttpClientMediator : IHttpClient
	{
		public Response Send(Request request)
		{
			var webRequest = MakeHttpWebRequest(request);
			return TryGetResponse(webRequest.GetResponse);
		}

		private static HttpWebRequest MakeHttpWebRequest(Request request)
		{
			var httpWebRequest = RequestForUrl(request.Url);

			httpWebRequest.Method = request.Method.ToString().ToUpperInvariant();
			httpWebRequest.UserAgent = "7digital .Net Api Wrapper";
			httpWebRequest.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip");
			httpWebRequest.Accept = request.Headers.ContainsKey("Accept") 
				? request.Headers["Accept"] 
				: "application/xml"; 

			foreach (var header in request.Headers.Where(header => header.Key != "Accept"))
			{
				httpWebRequest.Headers.Add(header.Key, header.Value);
			}

			if (request.Method.ShouldHaveRequestBody())
			{
				var contentType = request.Body.ContentType;
				var data = request.Body.Data;

				httpWebRequest.ContentType = contentType;
				var postBytes = Encoding.UTF8.GetBytes(data);
				httpWebRequest.ContentLength = postBytes.Length;

				using (var dataStream = httpWebRequest.GetRequestStream())
				{
					dataStream.Write(postBytes, 0, postBytes.Length);
				}
			}

			return httpWebRequest;
		}

		private static HttpWebRequest RequestForUrl(string url)
		{
			try
			{
				var uri = new Uri(url);
				var webRequest = WebRequest.Create(uri);
				return (HttpWebRequest)webRequest;
			}
			catch (Exception ex)
			{
				throw new InvalidOperationException("Could not create HttpWebRequest for url " + url, ex);
			}
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

			using (webResponse)
			{
				return MakeResponse(webResponse);
			}
		}

		private Response MakeResponse(WebResponse webResponse)
		{
			string output;
			using (var sr = new StreamReader(GetResponseStream(webResponse)))
			{
				output = sr.ReadToEnd();
			}

			var statusCode = ReadStatusCode(webResponse);
			var headers = MapResponseHeaders(webResponse.Headers);

			var response = new Response(statusCode, headers, output);

			return response;
		}

		public Dictionary<string, string> MapResponseHeaders(WebHeaderCollection headerCollection)
		{
			var headers = new Dictionary<string, string>();

			for (var i = 0; i < headerCollection.Count; i++)
			{
				headers.Add(headerCollection.GetKey(i), string.Join(",", headerCollection.GetValues(i)));
			}

			return headers;
		}

		private Stream GetResponseStream(WebResponse webResponse)
		{
			string contentEncodingHeader = webResponse.Headers["Content-Encoding"];

			if (contentEncodingHeader != null && contentEncodingHeader == "gzip")
			{
				return new GZipStream(webResponse.GetResponseStream(), CompressionMode.Decompress);
			}

			return webResponse.GetResponseStream();
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