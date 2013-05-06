namespace SevenDigital.Api.Wrapper.Serialization
{
	public interface IApiResponseDetector
	{
		bool IsXml(string responseBody);
		bool IsXmlParsed(string responseBody);
		bool IsApiOkResponse(string responseBody);
		bool IsApiErrorResponse(string responseBody);
		bool IsServerError(int httpStatusCode);
		bool IsOAuthError(string responseBody);
	}
}