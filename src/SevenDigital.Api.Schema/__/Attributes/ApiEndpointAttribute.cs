using System;

namespace SevenDigital.Api.Schema.Attributes
{
	[AttributeUsage(AttributeTargets.Class)]
	public class ApiEndpointAttribute : Attribute
	{
		private readonly string _endpointUri;

		public ApiEndpointAttribute(string endpointUri)
		{
			_endpointUri = endpointUri;
		}

		public string EndpointUri
		{
			get { return _endpointUri; }
		}
	}
}