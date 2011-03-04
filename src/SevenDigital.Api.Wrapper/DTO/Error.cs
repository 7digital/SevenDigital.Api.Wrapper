using System;
using System.Xml.Serialization;

namespace SevenDigital.Api.Wrapper.DTO
{
	[Serializable]
	[XmlRoot("error")]
	public class Error
	{
		[XmlAttribute("code")]
		public int Code { get; set; }

		[XmlElement("errorMessage")]
		public string ErrorMessage { get; set; }
	}
}