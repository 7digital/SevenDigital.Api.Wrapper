using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using SevenDigital.Api.Schema.Releases;

namespace SevenDigital.Api.Schema
{
    [XmlRoot("packages")]
    [Serializable]
    public class PackageList
    {
        [XmlArray("packages")]
        [XmlArrayItem("package")]
        public List<Package> Packages { get; set; }
    }
}