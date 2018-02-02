using System;
using System.IO;
using Paramore.Brighter;

namespace Basic
{
    public class SalutationHandler : RequestHandler<SalutationCommand>, IDisposable
    {
        public override SalutationCommand Handle(SalutationCommand command)
        {
            Console.WriteLine($"Greetings, {command.Name}.");
            return base.Handle(command);
        }

        public void Dispose()
        {
            Console.WriteLine("I'm being disposed.");
        }
    }
}