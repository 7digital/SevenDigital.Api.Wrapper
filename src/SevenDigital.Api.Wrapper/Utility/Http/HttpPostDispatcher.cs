using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace SevenDigital.Api.Wrapper.Utility.Http
{
	public class HttpPostDispatcher : IRequestDispatcher
	{
		public const string METHOD = "POST";
		private readonly IWebClientFactory _webClientFactory;
		private string _parametersAsString = string.Empty;

		public string ParametersAsString
		{
			get { return _parametersAsString; }
			set { _parametersAsString = value; }
		}

		public HttpPostDispatcher(IWebClientFactory webClientFactory)
		{
			_webClientFactory = webClientFactory;
		}
		public HttpPostDispatcher(IWebClientFactory webClientFactory, string parametersAsString)
		{
			_webClientFactory = webClientFactory;
			ParametersAsString = parametersAsString;
		}

		public string Dispatch(string endpoint, Dictionary<string, string> headers)
		{
			using (var webClientWrapper = _webClientFactory.GetWebClient())
			{
				webClientWrapper.Encoding = Encoding.UTF8;
				webClientWrapper.Headers = new WebHeaderCollection();
				webClientWrapper.Headers[HttpRequestHeader.UserAgent] = "7digital .Net Api Wrapper";

				return webClientWrapper.UploadString(endpoint, METHOD, ParametersAsString);
			}
		}

		public Response<string> FullDispatch(string endpoint, Dictionary<string, string> headers)
		{
			throw new NotImplementedException();
		}

		public void DispatchAsync(string endpoint, Dictionary<string, string> headers, Action<string> payload)
		{
			throw new NotImplementedException();
		}
	}
}