using System;

namespace SevenDigital.Api.Schema
{
	public interface ITypeGenerator
	{
		Type GenerateType(string endpoint);
	}
}