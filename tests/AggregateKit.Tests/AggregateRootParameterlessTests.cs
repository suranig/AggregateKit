using System;
using Xunit;

namespace AggregateKit.Tests
{
    public class AggregateRootParameterlessTests
    {
        private class ParameterlessAggregate : AggregateRoot<Guid>
        {
            public ParameterlessAggregate() : base() { }
        }

        [Fact]
        public void ParameterlessConstructor_Initializes_No_DomainEvents()
        {
            // Act
            var aggregate = new ParameterlessAggregate();

            // Assert
            Assert.Empty(aggregate.DomainEvents);
            Assert.False(aggregate.HasDomainEvents);
        }
    }
}
