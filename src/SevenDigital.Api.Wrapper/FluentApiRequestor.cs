using System;

namespace SevenDigital.Api.Wrapper
{
    public class FluentApiRequestor<T> : IFluentApiRequestor<T> where T : class
    {
        private readonly Func<IFluentApi<T>> _factory;

        public FluentApiRequestor(Func<IFluentApi<T>> factory)
        {
            _factory = factory;
        }

        public T Request(Action<IFluentApi<T>> action)
        {
            var fluentApi = _factory();
            action(fluentApi);
            return fluentApi.Please();
        }
    }
}
