using System.Collections.Specialized;
using System.Text;

namespace SevenDigital.Api.Wrapper.EndpointResolution
{
	public static class NameValueCollectionExtensions
	{
		public static string ToQueryString(this NameValueCollection collection)
		{
			var sb = new StringBuilder();
			foreach (string key in collection)
				sb.AppendFormat("{0}={1}&", key, collection[key]);

			return sb.ToString().Trim('&');
		}
	}
}