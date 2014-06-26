﻿using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using SevenDigital.Api.Schema.Tracks;

namespace SevenDigital.Api.Schema.Lockers
{
	[Serializable]
	[XmlRoot("lockerTrack")]
	public class LockerTrack
	{
		[XmlElement("track")]
		public Track Track { get; set; }

		[XmlElement("remainingDownloads")]
		public int RemainingDownloads { get; set; }

		[XmlElement("purchaseDate")]
		public DateTime PurchaseDate { get; set; }

		[XmlElement("purchaseId")]
		public int PurchaseId { get; set; }

		[XmlArray("downloadUrls")]
		[XmlArrayItem("downloadUrl")]
		public List<DownloadUrl> DownloadUrls { get; set; }
	}
}