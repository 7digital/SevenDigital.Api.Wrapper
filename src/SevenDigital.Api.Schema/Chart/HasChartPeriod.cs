using System;
using System.Collections.Generic;

namespace SevenDigital.Api.Schema.Chart
{
    public interface HasChartPeriod
    {
        ChartType Type { get; set; }
        DateTime FromDate { get; set; }
        DateTime ToDate { get; set; }
    }
    public interface IHasChartPeriod<T> : HasChartPeriod
    {
        List<T> ChartItems { get; set; }
    }
}