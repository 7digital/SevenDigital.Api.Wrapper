using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using SevenDigital.Api.Wrapper.Requests;
using SevenDigital.Api.Wrapper.Responses;

namespace SevenDigital.Api.Wrapper.Http
{
	public class HttpClientMediator : IHttpClient
	{
		private readonly IHttpClientHandlerFactory _factory;

		public HttpClientMediator() : this(new HttpClientHandlerFactory())
		{
		}

		public HttpClientMediator(IHttpClientHandlerFactory factory)
		{
			_factory = factory;
		}

		public async Task<Response> Send(Request request)
		{
			using (var httpClient = MakeHttpClient())
			{
				using (var httpRequest = MakeHttpRequest(request))
				{
					using (var httpResponse = await httpClient.SendAsync(httpRequest))
					{
						return await MakeResponse(httpResponse, request);
					}
				}
			}
		}

		private HttpClient MakeHttpClient()
		{
			var handler = _factory.CreateHandler();
			handler.AutomaticDecompression = DecompressionMethods.GZip;
			var httpClient = new HttpClient(handler);

			httpClient.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip", 1.0));
			httpClient.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("UTF8", 0.9));

			httpClient.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("7digital-.Net-Api-Wrapper", "4.0"));

			return httpClient;
		}

		private static HttpRequestMessage MakeHttpRequest(Request request)
		{
			var httpRequest = new HttpRequestMessage(request.Method, request.Url);

			foreach (var header in request.Headers)
			{
				httpRequest.Headers.TryAddWithoutValidation(header.Key, header.Value);
			}

			if (request.Method.ShouldHaveRequestBody())
			{
				HttpContent content = new StringContent(request.Body.Data);
				content.Headers.ContentType = new MediaTypeHeaderValue(request.Body.ContentType);
				httpRequest.Content = content;
			}

			return httpRequest;
		}

		private static async Task<Response> MakeResponse(HttpResponseMessage httpResponse, Request request)
		{
			var headers = new Dictionary<string, string>();
			AddResponseHeaders(httpResponse.Headers, headers);
			AddResponseHeaders(httpResponse.Content.Headers, headers);

			string responseBody = await httpResponse.Content.ReadAsStringAsync();
			return new Response(httpResponse.StatusCode, headers, responseBody, request);
		}

		private static void AddResponseHeaders(HttpHeaders sourceHeaderCollection, IDictionary<string, string> responseHeaders)
		{
			foreach (var header in sourceHeaderCollection)
			{
				responseHeaders.Add(header.Key, string.Join(",", header.Value));
			}
		}
	}
}
