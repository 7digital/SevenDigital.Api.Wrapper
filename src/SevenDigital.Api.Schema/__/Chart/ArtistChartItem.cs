using System;
using System.Xml.Serialization;
using SevenDigital.Api.Schema.ArtistEndpoint;
using SevenDigital.Api.Schema.ReleaseEndpoint;
using SevenDigital.Api.Schema.TrackEndpoint;

namespace SevenDigital.Api.Schema.Chart
{
	
	[XmlRoot("chartItem")]
	public class ArtistChartItem
	{
		[XmlElement("position")]
		public int Position { get; set; }

		[XmlElement("change")]
		public ChartItemChange Change { get; set; }

		[XmlElement("artist")]
		public Artist Artist { get; set; }
	}

	
	[XmlRoot("chartItem")]
	public class ReleaseChartItem
	{
		[XmlElement("position")]
		public int Position { get; set; }

		[XmlElement("change")]
		public ChartItemChange Change { get; set; }

		[XmlElement("release")]
		public Release Release { get; set; }
	}

	
	[XmlRoot("chartItem")]
	public class TrackChartItem
	{
		[XmlElement("position")]
		public int Position { get; set; }

		[XmlElement("change")]
		public ChartItemChange Change { get; set; }

		[XmlElement("track")]
		public Track Track { get; set; }
	}
}