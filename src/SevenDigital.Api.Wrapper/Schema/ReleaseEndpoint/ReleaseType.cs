using System;

namespace SevenDigital.Api.Wrapper.Schema.ReleaseEndpoint
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