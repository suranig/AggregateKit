using System;
using System.Collections.Generic;
using System.Linq;

namespace AggregateKit
{
    /// <summary>
    /// Base class for all Value Objects in Domain-Driven Design.
    /// Value Objects are equality-comparable by value, not by reference.
    /// </summary>
    public abstract class ValueObject : IEquatable<ValueObject>
    {
        /// <summary>
        /// When overridden in a derived class, returns all components of the value object
        /// that are used for equality comparison.
        /// </summary>
        /// <returns>Enumerable of all equality components.</returns>
        protected abstract IEnumerable<object?> GetEqualityComponents();

        public bool Equals(ValueObject? other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            if (GetType() != other.GetType()) return false;
            
            return GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as ValueObject);
        }

        public override int GetHashCode()
        {
            var hashCode = new HashCode();
            foreach (var component in GetEqualityComponents())
            {
                hashCode.Add(component);
            }
            return hashCode.ToHashCode();
        }

        public static bool operator ==(ValueObject? left, ValueObject? right)
        {
            return EqualityComparer<ValueObject>.Default.Equals(left, right);
        }

        public static bool operator !=(ValueObject? left, ValueObject? right)
        {
            return !(left == right);
        }
    }
} 