using System;
using System.Collections.Generic;

namespace SevenDigital.Api.Wrapper.EndpointResolution.Authorization
{
	public class QueryParameterComparer : IComparer<QueryParameter>
	{
		public int Compare(QueryParameter x, QueryParameter y)
		{
			return x.Name == y.Name 
			       	? String.Compare(x.Value, y.Value) 
			       	: String.Compare(x.Name, y.Name);
		}
	}
}