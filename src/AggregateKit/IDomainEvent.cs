using System;

namespace AggregateKit
{
    /// <summary>
    /// Interface for all domain events in Domain-Driven Design.
    /// </summary>
    public interface IDomainEvent
    {
        /// <summary>
        /// Gets the unique identifier for this domain event.
        /// </summary>
        Guid Id { get; }
        
        /// <summary>
        /// Gets the timestamp when this domain event occurred.
        /// </summary>
        DateTime OccurredOn { get; }
    }
} 