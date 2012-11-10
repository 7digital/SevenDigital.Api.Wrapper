using System;

namespace SevenDigital.Api.Wrapper
{
    public interface IFluentApiRequestor<T> where T : class
    {
        T Request(Action<IFluentApi<T>> action);
    }
}