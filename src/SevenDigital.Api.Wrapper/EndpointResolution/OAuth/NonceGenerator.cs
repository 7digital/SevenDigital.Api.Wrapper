using System;
using System.Security.Cryptography;

namespace SevenDigital.Api.Wrapper.EndpointResolution.OAuth
{
	public static class NonceGenerator
	{
		private static readonly RNGCryptoServiceProvider _rngCryptoServiceProvider = new RNGCryptoServiceProvider();

		public static string ThreadSafeNonce()
		{
			var data = new byte[4];
			_rngCryptoServiceProvider.GetBytes(data);
			return Math.Abs(BitConverter.ToInt32(data, 0)).ToString();
		}

		public static string OriginalNonceMethod()
		{
			return new Random().Next(123400, 9999999).ToString();
		}
	}
}