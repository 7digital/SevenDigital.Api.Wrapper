using System.Xml.Serialization;
using System.Collections.Generic;
using SevenDigital.Api.Schema.ArtistEndpoint;
using SevenDigital.Api.Schema.Attributes;
using SevenDigital.Api.Schema.ParameterDefinitions.Get;

namespace SevenDigital.Api.Schema.Merchandising
{
	[ApiEndpoint("merchandising/list/details")]
	[XmlRoot("merchandisingList")]
	public class MerchandisingList : HasKeyParameter
	{
		[XmlElement("key")]
		public string Key { get; set; }

		[XmlArray("merchandisingItems")]
		[XmlArrayItem("merchandisingItem")]
		public List<MerchandisingItem> MerchandisingItems { get; set; }
	}
}
