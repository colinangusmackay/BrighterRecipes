using System;
using System.Collections.Concurrent;
using Microsoft.Extensions.DependencyInjection;
using Paramore.Brighter;

namespace steps_and_timing
{
    public class ServiceProviderHandler : IAmAHandlerFactory
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ConcurrentDictionary<IHandleRequests, IServiceScope> _activeHandlers;
        public ServiceProviderHandler(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _activeHandlers = new ConcurrentDictionary<IHandleRequests, IServiceScope>();
        }
        public IHandleRequests Create(Type handlerType)
        {
            IServiceScope scope = _serviceProvider.CreateScope();
            IServiceProvider scopedProvider = scope.ServiceProvider;
            IHandleRequests result = (IHandleRequests)scopedProvider.GetService(handlerType);
            if (_activeHandlers.TryAdd(result, scope))
                return result;

            scope.Dispose();
            throw new InvalidOperationException("The handler could not be tracked properly. It may be declared in the service collection with the wrong lifecyle.");
        }

        public void Release(IHandleRequests handler)
        {
            if (_activeHandlers.TryRemove(handler, out IServiceScope scope))
            {
                scope.Dispose();
            }
        }
    }
}