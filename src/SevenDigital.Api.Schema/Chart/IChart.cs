using System;
using System.Collections.Generic;

namespace SevenDigital.Api.Schema.Chart
{
    public interface IChart
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