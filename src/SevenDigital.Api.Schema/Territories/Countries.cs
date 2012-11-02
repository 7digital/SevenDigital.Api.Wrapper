using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using SevenDigital.Api.Schema.Attributes;

namespace SevenDigital.Api.Schema.Territories
{
	[ApiEndpoint("countries")]
	[XmlRoot("countries")]
	[Serializable]
	public class Countries
	{
		[XmlElement("country")]
		public List<Country> CountryItems { get; set; }
	}
}
