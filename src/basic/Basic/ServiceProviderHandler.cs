using System;
using Paramore.Brighter;

namespace Basic
{
    public class ServiceProviderHandler : IAmAHandlerFactory
    {
        private readonly IServiceProvider _serviceProvider;
        public ServiceProviderHandler(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public IHandleRequests Create(Type handlerType)
        {
            return (IHandleRequests)_serviceProvider.GetService(handlerType);
        }

        public void Release(IHandleRequests handler)
        {
        }
    }
}