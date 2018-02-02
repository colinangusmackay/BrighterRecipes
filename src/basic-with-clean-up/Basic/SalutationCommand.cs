using System;
using Paramore.Brighter;

namespace Basic
{
    public class SalutationCommand : IRequest
    {
        public Guid Id { get; set; }

        public string Name { get; }

        public SalutationCommand(string name)
        {
            Id = Guid.NewGuid();
            Name = name;
        }
    }
}