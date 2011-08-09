using System;
using System.Collections.Generic;

namespace SevenDigital.Api.Schema.Chart
{
	public interface IIsChart
	{
		ChartType Type { get; set; }
		DateTime FromDate { get; set; }
		DateTime ToDate { get; set; }
	}
	public interface IIsChart<T> : IIsChart
	{
		List<T> ChartItems { get; set; }
	}
}