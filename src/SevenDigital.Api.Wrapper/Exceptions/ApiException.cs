using System;
using System.Runtime.Serialization;
using System.Security.Permissions;
using SevenDigital.Api.Wrapper.Requests;

namespace SevenDigital.Api.Wrapper.Exceptions
{
	public abstract class ApiException : Exception
	{
		public string Uri { get; internal set; }
		public Request OriginalRequest { get; private set; }

		protected ApiException (string msg, string uri, Exception innerException)
			: base (msg, innerException)
		{
			Uri = uri;
		}

		protected ApiException(string msg, Request originalRequest)
			: base(msg)
		{
			OriginalRequest = originalRequest;
		}

		protected ApiException(string msg, Exception innerException, Request originalRequest)
			: base(msg, innerException)
		{
			OriginalRequest = originalRequest;
		}

		protected ApiException(string msg) : base(msg)
		{
		}

		protected ApiException(string msg, Exception innerException)
			: base(msg, innerException)
		{
		}

		[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
		protected ApiException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			Uri = info.GetString("Uri");
			OriginalRequest = (Request)info.GetValue("OriginalRequest", typeof(Request));
		}

		[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}

			info.AddValue("Uri", Uri);
			info.AddValue("OriginalRequest", OriginalRequest);

			base.GetObjectData(info, context);
		}
	}
}