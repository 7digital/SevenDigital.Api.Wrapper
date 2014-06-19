using System.Globalization;

namespace SevenDigital.Api.Wrapper
{
	public static class ParameterExtensions
	{
		public static IFluentApi<T> WithIntParameter<T>(this IFluentApi<T> api, string name, int value)
		{
			api.WithParameter(name, value.ToString(CultureInfo.InvariantCulture));
			return api;
		}

		public static IFluentApi<T> WithDecimalParameter<T>(this IFluentApi<T> api, string name, decimal value)
		{
			api.WithParameter(name, value.ToString(CultureInfo.InvariantCulture));
			return api;
		}

	}
}
