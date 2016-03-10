namespace SevenDigital.Api.Wrapper.Requests
{
	public interface IRouteParamsSubstitutor
	{
		ApiRequest SubstituteParamsInRequest(RequestData requestData);
	}
}