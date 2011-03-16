using System;

namespace SevenDigital.Api.Wrapper.EndpointResolution.Authorization
{
	internal class NonceGenerator : IStringGenerator
	{
		public string Generate()
		{
			var random = new Random();
			return random.Next(123400, 9999999).ToString();
		}
	}
}