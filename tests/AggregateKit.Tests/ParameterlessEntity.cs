using System;

namespace AggregateKit.Tests
{
    public class ParameterlessEntity : Entity<Guid>
    {
        public ParameterlessEntity() : base()
        {
        }
    }
}

