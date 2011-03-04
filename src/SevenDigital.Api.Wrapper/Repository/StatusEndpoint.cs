using System.Xml;
using SevenDigital.Api.Wrapper.DTO;
using SevenDigital.Api.Wrapper.Utility.Serialization;

namespace SevenDigital.Api.Wrapper.Repository
{
	public class StatusEndpoint : IEndpointRepository<Status>
	{
		private readonly IEndpointResolver _endpointResolver;

		public StatusEndpoint(IEndpointResolver endpointResolver)
		{
			_endpointResolver = endpointResolver;
		}

		public Status Get()
		{
			// Build a fluent class for this!
			// like FluentApi.Get<Status>.WithMethod("GET").WithHeaders().Resolve();
			// or FLuentApi.Get<Artist>.WithParam("artistId").Equals("123").WithHeaders().Resolve();
			XmlNode output = _endpointResolver.HitEndpoint("status", "GET", null);
			var xmlSerializer = new XmlSerializer<Status>();
			Status status = xmlSerializer.DeSerialize(output);
			return status;
		}
	}
}