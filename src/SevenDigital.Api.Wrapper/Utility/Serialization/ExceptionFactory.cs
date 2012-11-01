using System;
using SevenDigital.Api.Schema;
using SevenDigital.Api.Wrapper.Exceptions;
using SevenDigital.Api.Wrapper.Utility.Http;

namespace SevenDigital.Api.Wrapper.Utility.Serialization
{
	public static class ExceptionFactory
	{
		public static ApiErrorException CreateApiErrorException(Error error, Response response)
		{
			ApiErrorException apiException;
			if (error.Code >= 1000 && error.Code < 2000)
			{
				apiException = new InputParameterException(error.ErrorMessage);
			}
			else if (error.Code >= 2000 && error.Code < 3000)
			{
				apiException = new InvalidResourceException(error.ErrorMessage);
			}
			else if (error.Code >= 3000 && error.Code < 4000)
			{
				apiException = new UserCardException(error.ErrorMessage);
			}
			else if ((error.Code >= 7000 && error.Code < 8000) || (error.Code >= 9000 && error.Code < 10000))
			{
				apiException = new RemoteApiException(error.ErrorMessage);
			}
			else
			{
				var unrecognisedErrorException = new UnrecognisedErrorException(error.ErrorMessage);
				PopulateStatusAndBodyFromResponse(response, unrecognisedErrorException);
				throw unrecognisedErrorException;
			}

			PopulateStatusAndBodyFromResponse(response, apiException);
			apiException.ErrorCode = error.Code;
			return apiException;
		}

		public static NonXmlResponseException CreateNonXmlResponseException(Response response)
		{
			var nonXmlResponseException = new NonXmlResponseException();
			PopulateStatusAndBodyFromResponse(response, nonXmlResponseException); 
			return nonXmlResponseException;
		}

		public static UnrecognisedStatusException CreateUnrecognisedStatusException(Response response)
		{
			var unrecognisedStatus = new UnrecognisedStatusException();
			PopulateStatusAndBodyFromResponse(response, unrecognisedStatus);
			return unrecognisedStatus;
		}

		public static UnrecognisedErrorException CreateUnrecognisedErrorException(Response response, Exception ex)
		{
			var unrecognisedErrorException = new UnrecognisedErrorException(UnrecognisedErrorException.DEFAULT_ERROR_MESSAGE, ex);
			PopulateStatusAndBodyFromResponse(response, unrecognisedErrorException);
			return unrecognisedErrorException;
		}

		public static OAuthException CreateOAuthException(Response response)
		{
			var oAuthException = new OAuthException();
			PopulateStatusAndBodyFromResponse(response, oAuthException);
			return oAuthException;
		}

		private static void PopulateStatusAndBodyFromResponse(Response response, ApiException apiException)
		{
			apiException.StatusCode = response.StatusCode;
			apiException.ResponseBody = response.Body;
		}
	}
}