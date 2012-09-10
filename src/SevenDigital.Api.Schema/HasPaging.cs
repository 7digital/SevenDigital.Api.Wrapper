﻿using System;
using System.Xml.Serialization;

namespace SevenDigital.Api.Schema
{
	[Serializable]
	public abstract class HasPaging : IHasPaging
	{
		[XmlElement("page")]
		public int Page { get; set; }

		[XmlElement("pageSize")]
		public int PageSize { get; set; }

		[XmlElement("totalItems")]
		public int TotalItems { get; set; }
	}

	public interface IHasPaging
	{
	}
}