using System;
using System.Collections.Generic;
using System.Linq;

namespace AggregateKit
{
    /// <summary>
    /// Base class for all Value Objects in Domain-Driven Design.
    /// Value Objects are equality-comparable by value, not by reference.
    /// </summary>
    public abstract class ValueObject
    {
        /// <summary>
        /// When overridden in a derived class, returns all components of the value object
        /// that are used for equality comparison.
        /// </summary>
        /// <returns>Enumerable of all equality components.</returns>
        protected abstract IEnumerable<object?> GetEqualityComponents();

        public override bool Equals(object? obj)
        {
            if (obj == null || obj.GetType() != GetType())
            {
                return false;
            }

            var other = (ValueObject)obj;
            
            return GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
        }

        public override int GetHashCode()
        {
            return GetEqualityComponents()
                .Select(x => x != null ? x.GetHashCode() : 0)
                .Aggregate((x, y) => x ^ y);
        }

        public static bool operator ==(ValueObject? left, ValueObject? right)
        {
            if (ReferenceEquals(left, null) && ReferenceEquals(right, null))
                return true;
                
            if (ReferenceEquals(left, null) || ReferenceEquals(right, null))
                return false;
                
            return left.Equals(right);
        }

        public static bool operator !=(ValueObject? left, ValueObject? right)
        {
            return !(left == right);
        }
    }
} 