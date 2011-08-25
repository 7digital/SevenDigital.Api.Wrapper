using System.Xml.Serialization;
using System.Collections.Generic;
using SevenDigital.Api.Schema.Attributes;
using SevenDigital.Api.Schema.ParameterDefinitions.Get;

namespace SevenDigital.Api.Schema.Merchandising
{
	[ApiEndpoint("merchandising/list/details")]
	[XmlRoot("List")]
	public class MerchandisingList : HasKeyParameter
	{
		[XmlElement("key")]
		public string Key { get; set; }

		[XmlArray("Items")]
		[XmlArrayItem("Item")]
		public List<ListItem> Items { get; set; }
	}
}
