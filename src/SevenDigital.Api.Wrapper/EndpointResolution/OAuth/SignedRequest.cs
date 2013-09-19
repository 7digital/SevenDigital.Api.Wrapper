namespace SevenDigital.Api.Wrapper.EndpointResolution.OAuth
{
	public class SignedRequest
	{
		public string NormalizedRequestParameters { get; set; }
		public string NormalizedUrl { get; set; }
		public string Signature { get; set; }
		public string Timestamp { get; set; }
		public string Nonce { get; set; }
	}
}