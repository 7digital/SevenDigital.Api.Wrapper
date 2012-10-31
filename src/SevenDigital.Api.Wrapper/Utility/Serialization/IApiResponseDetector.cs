namespace SevenDigital.Api.Wrapper.Utility.Serialization
{
	public interface IApiResponseDetector
	{
		bool IsXml(string responseBody);
		bool IsApiOkResponse(string responseBody);
		bool IsApiErrorResponse(string responseBody);
		bool IsServerError(int httpStatusCode);
	}
}