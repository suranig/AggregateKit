using System.Collections.Generic;
using System.Collections.Immutable;

namespace AggregateKit
{
    /// <summary>
    /// Base class for all Aggregate Roots in Domain-Driven Design.
    /// An Aggregate Root is a cluster of domain objects that can be treated as a single unit.
    /// </summary>
    /// <typeparam name="TId">The type of the identity of the aggregate root.</typeparam>
    public abstract class AggregateRoot<TId> : Entity<TId> where TId : notnull
    {
        private readonly List<IDomainEvent> _domainEvents = [];
        
        /// <summary>
        /// Gets the domain events that have been raised by this aggregate root.
        /// </summary>
        public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

        protected AggregateRoot(TId id) : base(id) 
        {
        }

        // EF Core requires a parameterless constructor
        protected AggregateRoot() 
        {
        }

        /// <summary>
        /// Adds a domain event to this aggregate root.
        /// </summary>
        /// <param name="domainEvent">The domain event to add.</param>
        protected void AddDomainEvent(IDomainEvent domainEvent)
        {
            Guard.AgainstNull(domainEvent);
            _domainEvents.Add(domainEvent);
        }

        /// <summary>
        /// Clears all domain events from this aggregate root.
        /// This is typically called after all events have been published.
        /// </summary>
        public void ClearDomainEvents()
        {
            _domainEvents.Clear();
        }

        /// <summary>
        /// Gets a value indicating whether this aggregate root has any domain events.
        /// </summary>
        public bool HasDomainEvents => _domainEvents.Count > 0;
    }
} 