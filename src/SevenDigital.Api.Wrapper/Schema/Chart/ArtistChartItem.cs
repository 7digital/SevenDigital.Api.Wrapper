using System;
using System.Xml.Serialization;
using SevenDigital.Api.Wrapper.Schema.ArtistEndpoint;
using SevenDigital.Api.Wrapper.Schema.ReleaseEndpoint;
using SevenDigital.Api.Wrapper.Schema.TrackEndpoint;

namespace SevenDigital.Api.Wrapper.Schema.Chart
{
	[Serializable]
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

	[Serializable]
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

	[Serializable]
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