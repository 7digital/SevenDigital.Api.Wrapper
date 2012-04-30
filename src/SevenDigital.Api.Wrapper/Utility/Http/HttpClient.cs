using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using SevenDigital.Api.Wrapper.EndpointResolution;

namespace SevenDigital.Api.Wrapper.Utility.Http
{
	public class HttpClient : IHttpClient
	{
		public IResponse Get(IRequest request)
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
				if (ex.Response == null)
					throw;
				webResponse = ex.Response;
			}

			var output = string.Empty;
			using (var sr = new StreamReader(webResponse.GetResponseStream()))
			{
				output = sr.ReadToEnd();
			}

			var response = new Response
							{
								Body = output,
								Headers = MapHeaders(webResponse.Headers)
							};

			return response;
		}

		public void GetAsync(IRequest request, Action<IResponse> callback)
		{
			var client = new WebClient();
			client.DownloadStringCompleted += (s, e) =>
												{
													var response = new Response()
																	{
																		Body = e.Result,
																		Headers = MapHeaders(client.ResponseHeaders)
																	};
													callback(response);
												};
			client.DownloadStringAsync(new Uri(request.Url));
		}

		public IResponse Post(IRequest request)
		{
			var client = new WebClient();

			client.Headers.Add(HttpRequestHeader.ContentType, "application/x-www-form-urlencoded");

			string output;
			try
			{
				output = client.UploadString(request.Url, request.Parameters.ToQueryString());
			}
			catch (WebException ex)
			{
				if (ex.Response == null)
					throw;

				using (var sr = new StreamReader(ex.Response.GetResponseStream()))
				{
					output = sr.ReadToEnd();
				}
			}

			var response = new Response
			{
				Body = output,
			};

			return response;
		}

		public void PostAsync(IRequest request, Action<IResponse> callback)
		{
			var client = new WebClient();

			client.UploadStringCompleted += (s, e) =>
			{
				var response = new Response()
				{
					Body = e.Result,
					Headers = MapHeaders(client.ResponseHeaders)
				};
				callback(response);
			};
			
			
			client.Headers.Add(HttpRequestHeader.ContentType, "application/x-www-form-urlencoded");
			client.UploadStringAsync(new Uri(request.Url), request.Parameters.ToQueryString());
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
}