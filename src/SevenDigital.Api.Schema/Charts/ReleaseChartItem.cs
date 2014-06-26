using System.Xml.Serialization;
using SevenDigital.Api.Schema.Releases;

namespace SevenDigital.Api.Schema.Charts
{
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
}