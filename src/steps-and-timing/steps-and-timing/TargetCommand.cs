using System;
using Paramore.Brighter;

namespace steps_and_timing
{
    public class TargetCommand : IRequest
    {
        public Guid Id { get; set; }

        public TargetCommand()
        {
            Id = Guid.NewGuid();
        }
    }
}