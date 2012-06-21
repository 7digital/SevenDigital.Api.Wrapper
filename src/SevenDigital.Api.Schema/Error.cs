using System.Xml.Serialization;

namespace SevenDigital.Api.Schema
{
	[XmlRoot("error")]
	public class Error
	{
		[XmlAttribute("code")]
		public int Code { get; set; }

		[XmlElement("errorMessage")]
		public string ErrorMessage { get; set; }
	}
}