﻿using System;
using Paramore.Brighter;

namespace QualityOfService
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