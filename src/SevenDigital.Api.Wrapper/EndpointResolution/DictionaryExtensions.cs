using System.Collections.Generic;
using System.Net;
using System.Text;

namespace SevenDigital.Api.Wrapper.EndpointResolution
{
	public static class DictionaryExtensions
	{
        public static string ToQueryString(this Dictionary<string,string> collection)
		{
            var sb = new StringBuilder();
            foreach (var key in collection.Keys)
            {
                sb.AppendFormat("{0}={1}&", key, collection[key]);
            }
            return sb.ToString().TrimEnd('&');
		}
	}
}