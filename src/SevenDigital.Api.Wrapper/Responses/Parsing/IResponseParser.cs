namespace SevenDigital.Api.Wrapper.Responses.Parsing
{
	public interface IResponseParser
	{
		T Parse<T>(Response response) where T : class, new();
	}
}