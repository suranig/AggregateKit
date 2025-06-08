using System;
using System.Collections.Generic;

namespace AggregateKit
{
    /// <summary>
    /// Base class for all Entities in Domain-Driven Design.
    /// Entities are equality-comparable by identity, not by attributes.
    /// </summary>
    /// <typeparam name="TId">The type of the identity of the entity.</typeparam>
    public abstract class Entity<TId> : IEquatable<Entity<TId>> where TId : notnull
    {
        public TId Id { get; protected set; } = default!;

        protected Entity(TId id)
        {
            ArgumentNullException.ThrowIfNull(id);
            
            if (EqualityComparer<TId>.Default.Equals(id, default))
            {
                throw new ArgumentException("The ID cannot be the default value.", nameof(id));
            }

            Id = id;
        }

        // EF Core requires a parameterless constructor
        protected Entity() { }

        public bool Equals(Entity<TId>? other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            if (GetType() != other.GetType()) return false;
            
            return EqualityComparer<TId>.Default.Equals(Id, other.Id);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as Entity<TId>);
        }

        public override int GetHashCode()
        {
            return EqualityComparer<TId>.Default.GetHashCode(Id);
        }

        public static bool operator ==(Entity<TId>? left, Entity<TId>? right)
        {
            return EqualityComparer<Entity<TId>>.Default.Equals(left, right);
        }

        public static bool operator !=(Entity<TId>? left, Entity<TId>? right)
        {
            return !(left == right);
        }
    }
} 