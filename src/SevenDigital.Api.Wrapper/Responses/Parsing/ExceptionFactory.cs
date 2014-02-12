using SevenDigital.Api.Schema;
using SevenDigital.Api.Wrapper.Exceptions;

namespace SevenDigital.Api.Wrapper.Responses.Parsing
{
	public static class ExceptionFactory
	{
		public static ApiErrorException CreateApiErrorException(Error error, Response response)
		{
			var errorCode = (ErrorCode) error.Code;
			ApiErrorException apiException;

			if (error.Code >= 1000 && error.Code < 2000)
			{
				apiException = new InputParameterException(error.ErrorMessage, response, errorCode);
			}
			else if (error.Code >= 2000 && error.Code < 3000)
			{
				apiException = new InvalidResourceException(error.ErrorMessage, response, errorCode);
			}
			else if (error.Code >= 3000 && error.Code < 4000)
			{
				apiException = new UserCardException(error.ErrorMessage, response, errorCode);
			}
			else if ((error.Code >= 7000 && error.Code < 8000) || (error.Code >= 9000 && error.Code < 10000))
			{
				apiException = new RemoteApiException(error.ErrorMessage, response, errorCode);
			}
			else
			{
				throw new UnrecognisedErrorException(error.ErrorMessage, response);
			}

			return apiException;
		}
	}
}