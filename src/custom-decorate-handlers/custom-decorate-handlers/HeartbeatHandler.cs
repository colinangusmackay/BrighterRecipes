using System;
using Newtonsoft.Json;
using Paramore.Brighter;

namespace CustomDecorateHandlers
{
    public class HeartbeatHandler<TRequest> : RequestHandler<TRequest> where TRequest : class, IRequest
    {
        public override TRequest Handle(TRequest command)
        {
            // We would probably call a heartbeat service at this point.
            // But for demonstration we'll just write to the console.

            Console.WriteLine($"Heartbeat pulsed for {command.GetType().FullName}");
            string jsonString = JsonConvert.SerializeObject(command);
            Console.WriteLine(jsonString);

            return base.Handle(command);
        }
    }
}