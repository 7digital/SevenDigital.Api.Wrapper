using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using SevenDigital.Api.Schema.Attributes;

namespace SevenDigital.Api.Schema.Translations
{
	[ApiEndpoint("translations")]
	[DataContract(Name="translations")]
	[Serializable]
	public class Translations : HasPaging
	{
		[XmlElement("translation")]
		[DataMember(Name="translation")]
		public List<Translation> TranslationItems { get; set; }
	}

	public class Translation
	{
		[XmlElement("key")]
		public string Key { get; set; }

		[XmlElement("translatedText")]
		public string TranslatedText { get; set; }
	}
}
