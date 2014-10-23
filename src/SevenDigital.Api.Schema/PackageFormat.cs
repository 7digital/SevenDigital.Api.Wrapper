using System;
using System.Xml.Serialization;

namespace SevenDigital.Api.Schema
{
    [Serializable]
    public class PackageFormat
    {
        [XmlAttribute("id")]
        public int Id { get; set; }

        [XmlElement("description")]
        public string Description { get; set; }
    }
}