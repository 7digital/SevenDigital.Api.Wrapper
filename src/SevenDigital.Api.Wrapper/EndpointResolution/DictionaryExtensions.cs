using System;
using System.Collections.Generic;
using System.Text;

namespace SevenDigital.Api.Wrapper.EndpointResolution
{
	public static class DictionaryExtensions
	{
        public static string ToQueryString(this IDictionary<string,string> collection)
		{
            var sb = new StringBuilder();
            foreach (var key in collection.Keys)
            {
                sb.AppendFormat("{0}={1}&", key, collection[key]);
            }
            return sb.ToString().TrimEnd('&');
		}

		public static string ToQueryString(this IDictionary<string, string> collection, bool shouldUrlEncode)
		{
			var sb = new StringBuilder();
			foreach (var key in collection.Keys)
			{
				var parameter = shouldUrlEncode 
								? Uri.EscapeDataString(collection[key]) 
								: collection[key];

				sb.AppendFormat("{0}={1}&", key, parameter);
			}
			return sb.ToString().TrimEnd('&');
		}
	}
}