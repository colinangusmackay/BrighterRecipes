using System;
using Paramore.Brighter;

namespace CustomDecorateHandlers
{
    public class HeartbeatAttribute : RequestHandlerAttribute
    {
        public HeartbeatAttribute(int step, HandlerTiming timing = HandlerTiming.After) : base(step, timing)
        {
        }

        public override Type GetHandlerType()
        {
            return typeof(HeartbeatHandler<>);
        }
    }
}