using System;
using System.Collections.Generic;
using SevenDigital.Api.Schema.ParameterDefinitions.Get;

namespace SevenDigital.Api.Schema.Chart
{
	public interface IChart : HasChartParameter
	{
		ChartType Type { get; set; }
		DateTime FromDate { get; set; }
		DateTime ToDate { get; set; }
	}
	public interface IChart<T> : IChart
	{
		List<T> ChartItems { get; set; }
	}
}