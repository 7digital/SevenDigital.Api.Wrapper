using System.Collections.Generic;
using System.Linq;

namespace SevenDigital.Api.Wrapper.EndpointResolution.RequestHandlers
{
	public class ApiRequest
	{
		public string AbsoluteUrl { get; set; }
		public Dictionary<string, string> Parameters { get; set; }

		public string FullUri
		{
			get
			{
				if(Parameters.Any())
					return AbsoluteUrl + "?" + Parameters.ToQueryString(true);

				return AbsoluteUrl;
			}
		}
	}
}