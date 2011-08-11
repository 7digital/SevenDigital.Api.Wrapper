using System.Xml.Serialization;
using SevenDigital.Api.Schema.TrackEndpoint;

namespace SevenDigital.Api.Schema.Chart
{
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