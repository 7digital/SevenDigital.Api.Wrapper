using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Runtime.Serialization;
using SevenDigital.Api.Wrapper.Http;

namespace SevenDigital.Api.Wrapper.Requests
{
	[Serializable]
	public class Request : ISerializable
	{
		public Request(HttpMethod method, string url, IDictionary<string, string> headers, RequestPayload body)
		{
			Method = method;
			Url = url;
			Headers = headers;
			Body = body;
		}

		public Request(SerializationInfo info, StreamingContext context)
		{
			Method = HttpMethodHelpers.Parse(info.GetString("Method"));
			Url = info.GetString("Url");
			Body = (RequestPayload)info.GetValue("Body", typeof(RequestPayload));
			Headers = (IDictionary<string, string>)info.GetValue("Headers", typeof(IDictionary<string, string>));
		}

		public HttpMethod Method { get; private set; }

		public string Url { get; private set; }
		public IDictionary<string, string> Headers { get; private set; }
		public RequestPayload Body { get; private set; }

		/// <summary>
		/// we need to override serialisation as 'System.Net.Http.HttpMethod' is not a serializable type
		/// and these objects are attached to exceptions
		/// </summary>

		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("Method", Method.ToString(), typeof(string));
			info.AddValue("Url", Url, typeof(string));
			info.AddValue("Body", Body);
			info.AddValue("Headers", Headers);
		}
	}
}