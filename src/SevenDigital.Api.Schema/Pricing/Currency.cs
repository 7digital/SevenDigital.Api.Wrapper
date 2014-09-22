﻿using System;
using System.Xml.Serialization;

namespace SevenDigital.Api.Schema.Pricing
{
	[XmlRoot("currency")]
	[Serializable]
	public class Currency
	{
		[XmlAttribute("code")]
		public string Code { get; set; }

        [XmlText]
	    public string Symbol { get; set; }
	}
}