using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using SevenDigital.Api.Wrapper.Schema.Attributes;
using SevenDigital.Api.Wrapper.Schema.Media;
using SevenDigital.Api.Wrapper.Schema.ReleaseEndpoint;
using SevenDigital.Api.Wrapper.Schema.TrackEndpoint;

namespace SevenDigital.Api.Wrapper.Schema.LockerEndpoint
{
    [Serializable]
    [ApiEndpoint("user/locker")]
    [XmlRoot("locker")]
    public class Locker: HasPaging
    {
        [XmlArray("lockerReleases")]
        [XmlArrayItem("lockerRelease")]
        public List<LockerRelease> LockerReleases { get; set; }
    }

    [XmlRoot("lockerRelease")]
    public class LockerRelease
    {
        [XmlElement("release")]
        public Release Release { get; set; }

        [XmlArray("lockerTracks")]
        [XmlArrayItem("lockerTrack")]
        public List<LockerTrack> LockerTracks { get; set; }
    }

    [XmlRoot("lockerTrack")]
    public class LockerTrack
    {
        [XmlElement("track")]
        public Track Track { get; set; }

        [XmlElement("remainingDownloads")]
        public int RemainingDownloads { get; set; }

        [XmlElement("purchaseDate")]
        public string PurchaseDate { get; set; }

        [XmlArray("downloadUrls")]
        [XmlArrayItem("downloadUrl")]
        public List<DownloadUrl> DownloadUrls { get; set; }
    }

    [XmlRoot("downloadUrl")]
    public class DownloadUrl
    {
        [XmlElement("url")]
        public string Url { get; set; }

        [XmlElement("format")]
        public Format Format { get; set; }
    }
}