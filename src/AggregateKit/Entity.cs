using System;

namespace AggregateKit
{
    /// <summary>
    /// Base class for all Entities in Domain-Driven Design.
    /// Entities are equality-comparable by identity, not by attributes.
    /// </summary>
    /// <typeparam name="TId">The type of the identity of the entity.</typeparam>
    public abstract class Entity<TId> where TId : notnull
    {
        public TId Id { get; protected set; } = default!;

        protected Entity(TId id)
        {
            if (object.Equals(id, default(TId)))
            {
                throw new ArgumentException("The ID cannot be the default value.", nameof(id));
            }

            Id = id;
        }

        // EF Core requires a parameterless constructor
        protected Entity() {}

        public override bool Equals(object? obj)
        {
            if (obj == null || obj.GetType() != GetType())
            {
                return false;
            }

            var other = (Entity<TId>)obj;
            
            return Id.Equals(other.Id);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public static bool operator ==(Entity<TId>? left, Entity<TId>? right)
        {
            if (ReferenceEquals(left, null) && ReferenceEquals(right, null))
                return true;
                
            if (ReferenceEquals(left, null) || ReferenceEquals(right, null))
                return false;
                
            return left.Equals(right);
        }

        public static bool operator !=(Entity<TId>? left, Entity<TId>? right)
        {
            return !(left == right);
        }
    }
} 