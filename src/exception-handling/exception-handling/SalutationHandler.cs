using System;
using Paramore.Brighter.Policies.Attributes;

namespace exception_handling
{
    public class SalutationHandler : MyRequestHandler<SalutationCommand>
    {
        [FallbackPolicy(backstop:true, circuitBreaker:false, step:1)]
        public override SalutationCommand Handle(SalutationCommand command)
        {
            Console.WriteLine($"Greetings, {command.Name}.");

            ThrowOnTheDarkLord(command);

            return base.Handle(command);
        }

        private static void ThrowOnTheDarkLord(SalutationCommand command)
        {
            if (command.Name == "Voldemort")
                throw new ApplicationException("A death-eater has appeared.");
        }

        protected override SalutationCommand ExceptionFallback(SalutationCommand command, Exception exception)
        {
            Console.WriteLine(exception);
            return base.ExceptionFallback(command, exception);
        }
    }
}