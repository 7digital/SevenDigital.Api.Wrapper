using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using SevenDigital.Api.Schema.Attributes;

namespace SevenDigital.Api.Schema.Translations
{
	[ApiEndpoint("translations")]
	[XmlRoot("translations")]
	[Serializable]
	public class TranslationList : HasPaging
	{
		[XmlElement("translation")]
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
