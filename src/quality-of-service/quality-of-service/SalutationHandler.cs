using System;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using Paramore.Brighter.Policies.Attributes;

namespace QualityOfService
{
    public class SalutationHandler : MyRequestHandler<SalutationCommand>
    {
        private int _attempt = 0;
        public SalutationHandler()
        {
            Console.WriteLine($"{GetType().FullName}.ctor");
        }
        
        [FallbackPolicy(step:1, backstop:true, circuitBreaker:false)]
        [UsePolicy(policy: "GreetingRetryPolicy", step:2)]
        public override SalutationCommand Handle(SalutationCommand command)
        {
            var greeting = GetGreeting(command);            
            ThrowOnTheDarkLord(command);
            
            Console.WriteLine(greeting);

            return base.Handle(command);
        }

        private string GetGreeting(SalutationCommand command)
        {
            ThrowOnFailureToRetrieveGreeting(command);
            return $"Hello, {command.Name}";
        }

        private void ThrowOnFailureToRetrieveGreeting(SalutationCommand command)
        {
            _attempt++;
            int numFailures = Math.Max((command.Name.Length - 3) / 2, 0);
            if (_attempt % (numFailures + 1) != 0)
                throw new ApplicationException($"While trying to greet {command.Name} a failure happend. I'm on attempt {_attempt}, and I will fail {numFailures} times.");
        }

        private static void ThrowOnTheDarkLord(SalutationCommand command)
        {
            if (command.Name == "Voldemort")
                throw new ApplicationException("A death-eater has appeared.");
        }

        protected override SalutationCommand ExceptionFallback(SalutationCommand command, Exception exception)
        {
            Console.WriteLine($"Still failed after {_attempt} attempts.");
            Console.WriteLine(exception);
            return base.ExceptionFallback(command, exception);
        }
    }
}