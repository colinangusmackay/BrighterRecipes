using System;
using Microsoft.Extensions.DependencyInjection;
using Paramore.Brighter;

namespace Basic
{
    class Program
    {
        static void Main(string[] args)
        {
            var serviceProvider = BuildServiceProvider();
            var commandProcessor = BuildCommandProcessor(serviceProvider);

            commandProcessor.Send(new SalutationCommand("Christian"));

            Console.ReadLine();
        }

        private static IServiceProvider BuildServiceProvider()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddTransient<SalutationHandler>();
            return serviceCollection.BuildServiceProvider();
        }

        private static IAmACommandProcessor BuildCommandProcessor(IServiceProvider serviceProvider)
        {
            var registry = CreateRegistry(); // 1. Maps commands to Handlers
            var factory = new ServiceProviderHandler(serviceProvider); // Builds handlers

            var builder = CommandProcessorBuilder.With()
                .Handlers(new HandlerConfiguration(
                    subscriberRegistry: registry,
                    handlerFactory: factory))
                .DefaultPolicy()
                .NoTaskQueues()
                .RequestContextFactory(new InMemoryRequestContextFactory());

            return builder.Build();
        }

        private static SubscriberRegistry CreateRegistry()
        {
            var registry = new SubscriberRegistry();
            registry.Register<SalutationCommand, SalutationHandler>();
            return registry;
        }
    }
}
