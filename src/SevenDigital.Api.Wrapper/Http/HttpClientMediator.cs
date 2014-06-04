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
			var httpClient = MakeHttpClient();
			var httpRequest = MakeHttpRequest(request);

			var httpResponse = await httpClient.SendAsync(httpRequest);
			return await MakeResponse(httpResponse);
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
				httpRequest.Headers.Add(header.Key, header.Value);
			}

			if (request.Method.ShouldHaveRequestBody())
			{
				HttpContent content = new StringContent(request.Body.Data);
				content.Headers.ContentType = new MediaTypeHeaderValue(request.Body.ContentType);
				httpRequest.Content = content;
			}

			return httpRequest;
		}

		private static async Task<Response> MakeResponse(HttpResponseMessage httpResponse)
		{
			var headers = MapHeaders(httpResponse.Headers);
			string responseBody = await httpResponse.Content.ReadAsStringAsync();
			return new Response(httpResponse.StatusCode, headers, responseBody);
		}

		private static IDictionary<string, string> MapHeaders(HttpHeaders headerCollection)
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