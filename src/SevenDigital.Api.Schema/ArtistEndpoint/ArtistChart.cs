using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using SevenDigital.Api.Schema.Attributes;
using SevenDigital.Api.Schema.Chart;

namespace SevenDigital.Api.Schema.ArtistEndpoint
{
	[Serializable]
	[ApiEndpoint("artist/chart")]
	[XmlRoot("chart")]
    public class ArtistChart : HasPaging, IChart<ArtistChartItem>
	{
		[XmlElement("type")]
		public ChartType Type { get; set; }

		[XmlElement("fromDate")]
		public DateTime FromDate { get; set; }

		[XmlElement("toDate")]
		public DateTime ToDate { get; set; }

		[XmlElement("chartItem")]
		public List<ArtistChartItem> ChartItems { get; set; }
	}
}