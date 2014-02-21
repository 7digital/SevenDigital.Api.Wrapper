using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using SevenDigital.Api.Schema.Attributes;
using SevenDigital.Api.Schema.ParameterDefinitions.Get;

namespace SevenDigital.Api.Schema.Merchandising
{
	[ApiEndpoint("merchandising/list")]
	[XmlRoot("list")]
	[Serializable]
	public class MerchandisingList : HasKeyParameter
	{
		[XmlElement("key")]
		public string Key { get; set; }

		[XmlArray("listItems")]
		[XmlArrayItem("listItem")]
		public List<ListItem> Items { get; set; }
	}
}
