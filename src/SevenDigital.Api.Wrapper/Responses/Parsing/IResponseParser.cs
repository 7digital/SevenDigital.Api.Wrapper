namespace SevenDigital.Api.Wrapper.Responses.Parsing
{
	public interface IResponseParser<out T>
	{
		T Parse(Response response);
	}
}