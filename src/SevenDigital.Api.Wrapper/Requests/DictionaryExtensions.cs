﻿using System.Collections.Generic;
using System.Text;
using OAuth;

namespace SevenDigital.Api.Wrapper.Requests
{
	public static class DictionaryExtensions
	{
		public static string ToQueryString(this IDictionary<string, string> collection)
		{
			var sb = new StringBuilder();
			foreach (var key in collection.Keys)
			{
				var parameter = OAuthTools.UrlEncodeStrict(collection[key]);
				sb.AppendFormat("{0}={1}&", key, parameter);
			}
			return sb.ToString().TrimEnd('&');
		}
	}
}