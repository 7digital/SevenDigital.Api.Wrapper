using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using SevenDigital.Api.Schema.Attributes;
using SevenDigital.Api.Schema.Chart;

namespace SevenDigital.Api.Schema.ReleaseEndpoint
{
	
	[ApiEndpoint("release/chart")]
	[XmlRoot("chart")]
    public class ReleaseChart : HasPaging, IChart<ReleaseChartItem>
	{
		[XmlElement("type")]
		public ChartType Type { get; set; }

		[XmlElement("fromDate")]
		public DateTime FromDate { get; set; }

		[XmlElement("toDate")]
		public DateTime ToDate { get; set; }

		[XmlElement("chartItem")]
		public List<ReleaseChartItem> ChartItems { get; set; }
	}
}