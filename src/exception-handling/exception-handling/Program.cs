using System;
using Microsoft.Extensions.DependencyInjection;
using Paramore.Brighter;
using Paramore.Brighter.Policies.Handlers;

namespace exception_handling
{
    class Program
    {
        static void Main(string[] args)
        {
            var serviceProvider = BuildServiceProvider();
            var commandProcessor = BuildCommandProcessor(serviceProvider);


            commandProcessor.Send(new SalutationCommand("Christian"));
            commandProcessor.Send(new SalutationCommand("Voldemort"));
            commandProcessor.Send(new SalutationCommand("Alisdair"));

            Console.ReadLine();
        }

        private static IServiceProvider BuildServiceProvider()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddScoped<SalutationHandler>();
            serviceCollection.AddScoped(typeof(FallbackPolicyHandler<>));
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
