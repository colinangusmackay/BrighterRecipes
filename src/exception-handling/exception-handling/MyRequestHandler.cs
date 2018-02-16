using System;
using Paramore.Brighter;
using Paramore.Brighter.Policies.Handlers;

namespace exception_handling
{
    public class MyRequestHandler<TCommand> : RequestHandler<TCommand> where TCommand : class, IRequest
    {
        public override TCommand Fallback(TCommand command)
        {
            if (this.Context.Bag
                .ContainsKey(FallbackPolicyHandler<TCommand>
                    .CAUSE_OF_FALLBACK_EXCEPTION))
            {
                Exception exception = (Exception)this.Context
                    .Bag[FallbackPolicyHandler<TCommand>
                        .CAUSE_OF_FALLBACK_EXCEPTION];
                return base.Fallback(ExceptionFallback(command, exception));
            }
            return base.Fallback(command);
        }

        protected virtual TCommand ExceptionFallback(TCommand command, Exception exception)
        {
            // If exceptions need to be handled, 
            // this should be implemented in a derived class
            return command;
        }
    }
}