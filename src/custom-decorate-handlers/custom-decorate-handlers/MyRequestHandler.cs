using System;
using System.Threading;
using Paramore.Brighter;
using Paramore.Brighter.Policies.Handlers;

namespace CustomDecorateHandlers
{
    public class MyRequestHandler<TCommand> : RequestHandler<TCommand> where TCommand : class, IRequest
    {
        public override TCommand Fallback(TCommand command)
        {
            Exception exception = GetException();
            if (exception != null)
                return base.Fallback(ExceptionFallback(command, exception));
            
            return base.Fallback(command);
        }

        protected virtual TCommand ExceptionFallback(TCommand command, Exception exception)
        {
            // If exceptions need to be handled, 
            // this should be implemented in a derived class
            return command;
        }

        /// <summary>
        /// Gets the Exception in the Context.Bag
        /// </summary>
        /// <returns>An exception or null</returns>
        protected Exception GetException()
        {
            string key = FallbackPolicyHandler<TCommand>.CAUSE_OF_FALLBACK_EXCEPTION;
            if (this.Context.Bag.ContainsKey(key))
            {
                Exception exception = (Exception)this.Context.Bag[key];
                return exception;
            }

            return null;
        }

        /// <summary>
        /// Gets the exception and unwraps it if it is an AggregateException
        /// </summary>
        /// <returns>The unwrapped exception, the actual exception, or null</returns>
        protected Exception GetUnwrappedException()
        {
            Exception exception = GetException();
            if (exception != null) 
                return exception.GetType() == typeof(AggregateException) 
                    ? exception.InnerException 
                    : exception;
            return null;
        }
    }
}