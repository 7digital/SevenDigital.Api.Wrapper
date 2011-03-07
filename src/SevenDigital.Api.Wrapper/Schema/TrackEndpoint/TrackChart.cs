using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using SevenDigital.Api.Wrapper.Schema.Attributes;
using SevenDigital.Api.Wrapper.Schema.Chart;

namespace SevenDigital.Api.Wrapper.Schema.TrackEndpoint
{
	[Serializable]
	[ApiEndpoint("track/chart")]
	[XmlRoot("chart")]
	public class TrackChart : HasPaging
	{
		[XmlElement("type")]
		public ChartType Type { get; set; }

		[XmlElement("fromDate")]
		public DateTime FromDate { get; set; }

		[XmlElement("toDate")]
		public DateTime ToDate { get; set; }

		[XmlElement("chartItem")]
		public List<TrackChartItem> ChartItems { get; set; }
	}
}