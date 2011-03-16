using System.Security.Cryptography;

namespace SevenDigital.Api.Wrapper.EndpointResolution.Authorization
{
	internal interface IHashComputer
	{
		string Compute(HashAlgorithm hashAlgorithm, string data);
	}
}