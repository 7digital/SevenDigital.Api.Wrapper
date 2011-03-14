using System.Security.Cryptography;

namespace SevenDigital.Api.Wrapper.EndpointResolution.Authorization
{
	public interface IHashComputer
	{
		string Compute(HashAlgorithm hashAlgorithm, string data);
	}
}