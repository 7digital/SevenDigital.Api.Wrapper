using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.Serialization;
using System.Security.Permissions;
using SevenDigital.Api.Wrapper.Requests;
using SevenDigital.Api.Wrapper.Responses;

namespace SevenDigital.Api.Wrapper.Exceptions
{
	public abstract class ApiResponseException : ApiException
	{
		public HttpStatusCode StatusCode { get; private set; }
		public string ResponseBody { get; private set; }
		public IDictionary<string, string> Headers { get; private set; }
		public Request OriginalRequest { get; private set; }

		protected ApiResponseException(string message, Response response)
			: base(message)
		{
			ResponseBody = response.Body;
			Headers = response.Headers;
			StatusCode = response.StatusCode;
			OriginalRequest = response.OriginalRequest;
		}

		protected ApiResponseException(string message, Exception innerException, Response response)
			: base(message, innerException)
		{
			ResponseBody = response.Body;
			Headers = response.Headers;
			StatusCode = response.StatusCode;
			OriginalRequest = response.OriginalRequest;
		}

		[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
		protected ApiResponseException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			StatusCode = (HttpStatusCode) info.GetValue("StatusCode", typeof (HttpStatusCode));
			ResponseBody = info.GetString("ResponseBody");
			Headers = (IDictionary<string, string>) info.GetValue("Headers", typeof (IDictionary<string, string>));
			OriginalRequest = (Request)info.GetValue("Request", typeof(Request));
		}

		[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}

			info.AddValue("StatusCode", StatusCode);
			info.AddValue("ResponseBody", ResponseBody);
			info.AddValue("Headers", Headers);
			info.AddValue("Request", OriginalRequest);

			base.GetObjectData(info, context);
		}
	}
}