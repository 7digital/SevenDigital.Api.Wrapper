using System;
using System.Security.Cryptography;
using System.Text;

namespace SevenDigital.Api.Wrapper.EndpointResolution.Authorization
{
	internal class HashComputer : IHashComputer
	{
		public string Compute(HashAlgorithm hashAlgorithm, string data)
		{
			if (hashAlgorithm == null)
				throw new ArgumentNullException("hashAlgorithm");

			if (string.IsNullOrEmpty(data))
				throw new ArgumentNullException("data");

			byte[] dataBuffer = Encoding.ASCII.GetBytes(data);
			byte[] hashBytes = hashAlgorithm.ComputeHash(dataBuffer);

			return Convert.ToBase64String(hashBytes);
		}
	}
}