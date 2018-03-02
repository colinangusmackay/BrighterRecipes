using System;
using Microsoft.Extensions.DependencyInjection;
using Paramore.Brighter;
using Paramore.Brighter.Policies.Handlers;
using Polly;

namespace QualityOfService
{
    class Program
    {
        public static void Main(string[] args)
        {
            var serviceProvider = BuildServiceProvider();
            var commandProcessor = BuildCommandProcessor(serviceProvider);


            commandProcessor.Send(new SalutationCommand("Zoë"));
            Console.WriteLine();Console.WriteLine();
            commandProcessor.Send(new SalutationCommand("René"));
            Console.WriteLine();Console.WriteLine();
            commandProcessor.Send(new SalutationCommand("Xavier"));
            Console.WriteLine();Console.WriteLine();
            commandProcessor.Send(new SalutationCommand("Alexandria"));
            Console.WriteLine();Console.WriteLine();
            commandProcessor.Send(new SalutationCommand("Voldemort"));
            Console.WriteLine();Console.WriteLine();
        }

        private static IServiceProvider BuildServiceProvider()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddScoped<SalutationHandler>();
            serviceCollection.AddScoped(typeof(FallbackPolicyHandler<>));
            serviceCollection.AddScoped(typeof(ExceptionPolicyHandler<>));
            return serviceCollection.BuildServiceProvider();
        }

        private static IAmACommandProcessor BuildCommandProcessor(IServiceProvider serviceProvider)
        {
            var registry = CreateRegistry(); 
            var factory = new ServiceProviderHandler(serviceProvider);
            var policies = GetPolicies();

            var builder = CommandProcessorBuilder.With()
                .Handlers(new HandlerConfiguration(
                    subscriberRegistry: registry,
                    handlerFactory: factory))
                .Policies(policies)
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

        private static IAmAPolicyRegistry GetPolicies()
        {
            var policyRegistry = new PolicyRegistry();

            // These are the default policies that must exist. 
            // We're not using them, so we're setting them to No-op
            policyRegistry.Add(CommandProcessor.RETRYPOLICY, Policy.NoOp());
            policyRegistry.Add(CommandProcessor.RETRYPOLICYASYNC, Policy.NoOpAsync());
            policyRegistry.Add(CommandProcessor.CIRCUITBREAKER, Policy.NoOp());
            policyRegistry.Add(CommandProcessor.CIRCUITBREAKERASYNC, Policy.NoOpAsync());
            
            var greetingRetryPolicy = Policy
                .Handle<Exception>()
                .WaitAndRetry(new[]
                {
                    TimeSpan.FromSeconds(1), 
                    TimeSpan.FromSeconds(2), 
                    TimeSpan.FromSeconds(4) 
                }, (exception, timeSpan) =>
                {
                    Console.WriteLine($" ** An error occurred: {exception.Message}");
                    Console.WriteLine($" ** Waiting {timeSpan.Seconds} seconds until retry.");
                });
        
            policyRegistry.Add("GreetingRetryPolicy", greetingRetryPolicy);
            return policyRegistry;
        }
    }
}
