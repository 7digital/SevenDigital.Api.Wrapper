using System.Xml.Serialization;
using SevenDigital.Api.Schema.ArtistEndpoint;

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
}