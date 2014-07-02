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

		private static HttpClient MakeHttpClient()
		{
			var httpClient = new HttpClient(new HttpClientHandler
			{
				AutomaticDecompression = DecompressionMethods.GZip
			});

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
			var headers = MapResponseHeaders(httpResponse.Headers);
			string responseBody = await httpResponse.Content.ReadAsStringAsync();
			return new Response(httpResponse.StatusCode, headers, responseBody, request);
		}

		private static IDictionary<string, string> MapResponseHeaders(HttpHeaders headerCollection)
		{
			var resultHeaders = new Dictionary<string, string>();

			foreach (var header in headerCollection)
			{
				resultHeaders.Add(header.Key, string.Join(",", header.Value));
			}

			return resultHeaders;
		}
	}
}