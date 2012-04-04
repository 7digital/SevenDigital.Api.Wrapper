using System;
using System.Collections.Generic;
using System.Net;

namespace SevenDigital.Api.Wrapper.Utility.Http
{
	public class HttpGetDispatcher : IRequestDispatcher
	{
		public string Dispatch(string endpoint, Dictionary<string, string> headers)
		{
			throw new NotSupportedException("Need to use async in windows mobile");
		}

		public Response<string> FullDispatch(string endpoint, Dictionary<string, string> headers)
		{
			throw new NotSupportedException("Need to use async in windows mobile");
		}

		public void DispatchAsync(string endpoint, Dictionary<string, string> headers, Action<string> payload)
		{
			var client = new WebClient();
			client.DownloadStringCompleted += (s, e) => payload(e.Result);
			client.DownloadStringAsync(new Uri(endpoint));
		}
	}
}
