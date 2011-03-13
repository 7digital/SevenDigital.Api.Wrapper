using System;

namespace SevenDigital.Api.Wrapper.EndpointResolution.Authorization
{
	public class TimeStampGenerator : IStringGenerator
	{
		public string Generate()
		{
			TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
			return Convert.ToInt64(ts.TotalSeconds).ToString();
		}
	}
}