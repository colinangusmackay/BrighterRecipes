using System;
using Paramore.Brighter;

namespace steps_and_timing
{
    public class StepsAndTimingHandler<TRequest> : RequestHandler<TRequest> where TRequest : class, IRequest
    {
        private int _step;
        private HandlerTiming _timing;
        public override void InitializeFromAttributeParams(params object[] initializerList)
        {
            _step = (int) initializerList[0];
            _timing = (HandlerTiming) initializerList[1];
        }

        public override TRequest Handle(TRequest command)
        {
            Console.WriteLine($"ENTER       : {this.GetType().Name} as step {_step} {_timing} the target handler.");
            var result = base.Handle(command);
            Console.WriteLine($"EXIT        : {this.GetType().Name} as step {_step} {_timing} the target handler.");
            return result;
        }
    }
}