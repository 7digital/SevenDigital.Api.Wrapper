namespace SevenDigital.Api.Schema
{
	public enum ErrorCode
	{
		Unknown = 0,

		RequiredParameterMissing = 1001,
		InvalidParameterValue = 1002,
		ParameterOutOfAllowableRange = 1003,
		InvalidEnumerationValue = 1006,

		ResourceNotFound = 2001,
		ResourceNotAvailableInCurrentContext = 2002,
		ResourceAlreadyExists = 2003,

		UserCardHasExpired = 3001,
		UserHasNoCardDetails = 3002,
		PaymentFailed = 3003,
		PriceSuppliedDoesNotMatch = 3004,
		PurchasesNotPermittedFromCardCountry = 3005,
		VoucherNotApplied = 3012,
		PaymentProviderConnectionError = 3100,
		PaymentFailedError = 3101,
		PaymentDeclinedError = 3102,
		AddCardFailedError = 3103,
		AddCardDeclinedError = 3104,
		RefundBasketNotAllowedError = 3105,
		RefundDeclinedError = 3106,
		TransactionAlreadyRefundedError = 3017,

		UnableToPerformAction = 7001,
		ApplicationConfigurationError = 7002,
		OperationTimedOut = 7003,
		RefundFailedError = 7102,

		UnexpectedInternalServerError = 9001
	}
}
