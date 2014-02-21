namespace SevenDigital.Api.Wrapper.Requests.Serializing
{
	public class JsonTransferContentType : ITransferContentType
	{
		public string ContentType { get { return "application/json"; } }

		public string Serialize<TPayload>(TPayload payload) where TPayload : class
		{
			throw new System.NotImplementedException();
		}
	}
}