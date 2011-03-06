using System;

namespace SevenDigital.Api.Wrapper.Schema
{
	[Serializable]
	public enum ChartType
	{
		artist,
		album,
		video,
		track
	}

	[Serializable]
	public enum ChartItemChange
	{
		Up,
		Down,
		Same,
		New
	}

}