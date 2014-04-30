using System;
using System.Runtime.Serialization;
using System.Security.Permissions;
using SevenDigital.Api.Wrapper.Requests;

namespace SevenDigital.Api.Wrapper.Exceptions
{
	public abstract class ApiException : Exception
	{
		public Request OriginalRequest { get; private set; }

		[Obsolete("This is due to be removed, please use OriginalRequest.Url")]
		public string Uri 
		{ 
			get { return OriginalRequest.Url; }
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
			OriginalRequest = (Request)info.GetValue("OriginalRequest", typeof(Request));
		}

		[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}

			info.AddValue("OriginalRequest", OriginalRequest);

			base.GetObjectData(info, context);
		}
	}
}