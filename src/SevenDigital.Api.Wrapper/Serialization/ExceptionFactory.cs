using SevenDigital.Api.Schema;
using SevenDigital.Api.Wrapper.Exceptions;
using SevenDigital.Api.Wrapper.Http;

namespace SevenDigital.Api.Wrapper.Serialization
{
	public static class ExceptionFactory
	{
		public static ApiErrorException CreateApiErrorException(Error error, Response response)
		{
			ApiErrorException apiException;
			if (error.Code >= 1000 && error.Code < 2000)
			{
				apiException = new InputParameterException(error.ErrorMessage, response, error);
			}
			else if (error.Code >= 2000 && error.Code < 3000)
			{
				apiException = new InvalidResourceException(error.ErrorMessage, response, error);
			}
			else if (error.Code >= 3000 && error.Code < 4000)
			{
				apiException = new UserCardException(error.ErrorMessage, response, error);
			}
			else if ((error.Code >= 7000 && error.Code < 8000) || (error.Code >= 9000 && error.Code < 10000))
			{
				apiException = new RemoteApiException(error.ErrorMessage, response, error);
			}
			else
			{
				throw new UnrecognisedErrorException(error.ErrorMessage, response);
			}

			return apiException;
		}
	}
}