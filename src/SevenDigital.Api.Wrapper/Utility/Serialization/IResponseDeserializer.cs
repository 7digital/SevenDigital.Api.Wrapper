using SevenDigital.Api.Wrapper.Utility.Http;

namespace SevenDigital.Api.Wrapper.Utility.Serialization
{
	public interface IResponseDeserializer<out T>
	{
		T Deserialize(IResponse response);
	}
}