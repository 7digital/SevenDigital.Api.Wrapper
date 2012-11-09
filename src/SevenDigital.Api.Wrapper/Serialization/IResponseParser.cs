using SevenDigital.Api.Wrapper.Http;

namespace SevenDigital.Api.Wrapper.Serialization
{
	public interface IResponseParser<out T>
	{
		T Parse(Response response);
	}
}