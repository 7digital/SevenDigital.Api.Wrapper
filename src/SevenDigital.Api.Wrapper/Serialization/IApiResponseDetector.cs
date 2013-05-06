using SevenDigital.Api.Wrapper.Http;

namespace SevenDigital.Api.Wrapper.Serialization
{
	public interface IApiResponseDetector
	{
		bool IsXml(string responseBody);
		void TestXmlParse(Response response);

		bool IsApiOkResponse(string responseBody);
		bool IsApiErrorResponse(string responseBody);
		bool IsServerError(int httpStatusCode);
		bool IsOAuthError(string responseBody);
	}
}