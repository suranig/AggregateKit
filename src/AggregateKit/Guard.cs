using System;
using System.Collections.Generic;
using System.Linq;

namespace AggregateKit
{
    /// <summary>
    /// Provides guard methods to validate arguments and state.
    /// </summary>
    public static class Guard
    {
        /// <summary>
        /// Ensures that the specified value is not null.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <exception cref="ArgumentNullException">Thrown when the value is null.</exception>
        public static void AgainstNull(object? value, string parameterName)
        {
            if (value == null)
            {
                throw new ArgumentNullException(parameterName);
            }
        }

        /// <summary>
        /// Ensures that the specified string is not null or empty.
        /// </summary>
        /// <param name="value">The string to check.</param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <exception cref="ArgumentException">Thrown when the string is null or empty.</exception>
        public static void AgainstNullOrEmpty(string? value, string parameterName)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentException("String cannot be null or empty.", parameterName);
            }
        }

        /// <summary>
        /// Ensures that the specified string is not null, empty, or consists only of whitespace.
        /// </summary>
        /// <param name="value">The string to check.</param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <exception cref="ArgumentException">Thrown when the string is null, empty, or consists only of whitespace.</exception>
        public static void AgainstNullOrWhiteSpace(string? value, string parameterName)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("String cannot be null, empty, or consist only of whitespace.", parameterName);
            }
        }

        /// <summary>
        /// Ensures that the specified collection is not null or empty.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the collection.</typeparam>
        /// <param name="value">The collection to check.</param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <exception cref="ArgumentException">Thrown when the collection is null or empty.</exception>
        public static void AgainstNullOrEmpty<T>(IEnumerable<T>? value, string parameterName)
        {
            AgainstNull(value, parameterName);
            
            if (!value.Any())
            {
                throw new ArgumentException("Collection cannot be empty.", parameterName);
            }
        }

        /// <summary>
        /// Ensures that the specified condition is true.
        /// </summary>
        /// <param name="condition">The condition to check.</param>
        /// <param name="message">The exception message to use when the condition is false.</param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <exception cref="ArgumentException">Thrown when the condition is false.</exception>
        public static void AgainstCondition(bool condition, string message, string parameterName)
        {
            if (condition)
            {
                throw new ArgumentException(message, parameterName);
            }
        }
    }
} 