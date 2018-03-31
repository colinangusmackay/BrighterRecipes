using System;
using Paramore.Brighter;

namespace steps_and_timing
{
    public class TargetHandler : BaseTargetHandler<TargetCommand>
    {
        [StepsAndTiming(step: 1, timing: HandlerTiming.Before)]
        [StepsAndTiming(step: 2, timing: HandlerTiming.Before)]
        [StepsAndTiming(step: 3, timing: HandlerTiming.Before)]
        [StepsAndTiming(step: 1, timing: HandlerTiming.After)]
        [StepsAndTiming(step: 2, timing: HandlerTiming.After)]
        [StepsAndTiming(step: 3, timing: HandlerTiming.After)]
        public override TargetCommand Handle(TargetCommand command)
        {
            Console.WriteLine($"ENTER       : {this.GetType().Name} as target handler.");
            var result = base.Handle(command);
            Console.WriteLine($"EXIT        : {this.GetType().Name} as target handler.");
            return result;
        }
    }
}