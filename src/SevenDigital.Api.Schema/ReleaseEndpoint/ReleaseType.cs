using System;

namespace SevenDigital.Api.Schema.ReleaseEndpoint
{
	[Serializable]
	public enum ReleaseType
	{
		Single,
		Album,
		Video,
		Exclusive,
		Item,
	}
}