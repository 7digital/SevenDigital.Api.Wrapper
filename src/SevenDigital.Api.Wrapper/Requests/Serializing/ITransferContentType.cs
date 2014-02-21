namespace SevenDigital.Api.Wrapper.Requests.Serializing
{
	public interface ITransferContentType
	{
		string ContentType { get; }
		string Serialize<TPayload>(TPayload payload) where TPayload : class;
	}
}