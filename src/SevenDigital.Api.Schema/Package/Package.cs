using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace SevenDigital.Api.Schema.Package
{
    [XmlRoot("package")]
    [Serializable]
    public class Package
    {
        [XmlAttribute("id")]
        public int Id { get; set; }

        [XmlElement("description")]
        public string Description { get; set; }

        [XmlElement("price")]
        public PackagePriceResponse PriceResponse { get; set; }

        [XmlArray("formats")]
        [XmlArrayItem("format")]
        public List<PackageFormat> Formats { get; set; }
    }
}