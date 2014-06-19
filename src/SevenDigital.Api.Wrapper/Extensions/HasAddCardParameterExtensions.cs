using SevenDigital.Api.Schema.ParameterDefinitions.Post;

namespace SevenDigital.Api.Wrapper
{
	public static class HasAddCardParameterExtensions
	{
		public static IFluentApi<T> WithCard<T>(this IFluentApi<T> api, AddCardParameters card) where T : HasAddCardParameter
		{
			api.WithParameter("cardNumber", card.Number);
			api.WithParameter("cardType", card.Type);
			api.WithParameter("cardHolderName", card.HolderName);
			
			api.WithParameter("cardExpiryDate", card.ExpiryDate.ToString("yyyyMM"));
			api.WithParameter("cardVerificationCode", card.VerificationCode);
			api.WithParameter("cardPostCode", card.PostCode);
			api.WithParameter("cardCountry", card.TwoLetterISORegionName);

			if (card.IssueNumber.HasValue)
			{
				api.WithIntParameter("cardIssueNumber", card.IssueNumber.Value);
			}

			if (card.StartDate.HasValue)
			{
				api.WithParameter("cardStartDate", card.StartDate.Value.ToString("yyyyMM"));
			}

			return api;
		}
	}
}