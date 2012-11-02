using SevenDigital.Api.Wrapper.Utility.Http;

namespace SevenDigital.Api.Wrapper.Utility.Serialization
{
	public interface IResponseParser<out T>
	{
		T Parse(Response response);
	}
}