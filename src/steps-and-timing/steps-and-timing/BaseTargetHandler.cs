using System;
using Paramore.Brighter;

namespace steps_and_timing
{
    public abstract class BaseTargetHandler<TCommand> : RequestHandler<TCommand> where TCommand : class, IRequest
    {
        [StepsAndTiming(step: int.MinValue, timing: HandlerTiming.Before)]
        public override TCommand Handle(TCommand command)
        {
            Console.WriteLine($"ENTER       : Base of {this.GetType().Name} as target handler.");
            var result = base.Handle(command);
            Console.WriteLine($"EXIT        : Base of {this.GetType().Name} as target handler.");
            return result;
        }
    }
}