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
            foreach (var item in collection)
            {
                sb.AppendFormat("{0}={1}&", item.Key, item.Value);
            }
            return sb.ToString().TrimEnd('&');
		}
	}
}