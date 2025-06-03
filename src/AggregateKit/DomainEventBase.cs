using System;

namespace AggregateKit
{
    /// <summary>
    /// Base class for all domain events in Domain-Driven Design.
    /// </summary>
    public abstract class DomainEventBase : IDomainEvent
    {
        /// <summary>
        /// Gets the unique identifier for this domain event.
        /// </summary>
        public Guid Id { get; }
        
        /// <summary>
        /// Gets the timestamp when this domain event occurred.
        /// </summary>
        public DateTime OccurredOn { get; }

        protected DomainEventBase()
        {
            Id = Guid.NewGuid();
            OccurredOn = DateTime.UtcNow;
        }
    }
} 