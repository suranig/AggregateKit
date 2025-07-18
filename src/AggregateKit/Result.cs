using System;
using System.Collections.Generic;
using System.Linq;

namespace AggregateKit
{
    /// <summary>
    /// Represents the result of an operation that can succeed or fail.
    /// </summary>
    public class Result
    {
        protected readonly List<string> _errors = [];

        /// <summary>
        /// Gets a value indicating whether the operation was successful.
        /// </summary>
        public bool IsSuccess => _errors.Count == 0;

        /// <summary>
        /// Gets a value indicating whether the operation failed.
        /// </summary>
        public bool IsFailure => !IsSuccess;

        /// <summary>
        /// Gets the errors that occurred during the operation.
        /// </summary>
        public IReadOnlyList<string> Errors => _errors.AsReadOnly();

        /// <summary>
        /// Initializes a new instance of the <see cref="Result"/> class.
        /// </summary>
        protected Result() { }

        /// <summary>
        /// Creates a new successful result.
        /// </summary>
        /// <returns>A successful result.</returns>
        public static Result Success() => new();

        /// <summary>
        /// Creates a new failed result with the specified error message.
        /// </summary>
        /// <param name="error">The error message.</param>
        /// <returns>A failed result.</returns>
        public static Result Failure(string error)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(error);
            
            var result = new Result();
            result._errors.Add(error);
            return result;
        }

        /// <summary>
        /// Creates a new failed result with the specified error messages.
        /// </summary>
        /// <param name="errors">The error messages.</param>
        /// <returns>A failed result.</returns>
        public static Result Failure(IEnumerable<string> errors)
        {
            ArgumentNullException.ThrowIfNull(errors);
            
            var errorList = errors.ToList();
            if (errorList.Count == 0)
            {
                throw new ArgumentException("At least one error must be provided.", nameof(errors));
            }

            var result = new Result();
            result._errors.AddRange(errorList);
            return result;
        }

        /// <summary>
        /// Creates a new failed result with the specified error messages.
        /// </summary>
        /// <param name="errors">The error messages.</param>
        /// <returns>A failed result.</returns>
        public static Result Failure(params string[] errors) => Failure((IEnumerable<string>)errors);
    }

    /// <summary>
    /// Represents the result of an operation that can succeed with a value or fail.
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    public class Result<T> : Result
    {
        private readonly T? _value;

        /// <summary>
        /// Gets the value of the result.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown when the result is a failure.</exception>
        public T Value
        {
            get
            {
                if (IsFailure)
                    throw new InvalidOperationException("Cannot access the value of a failed result.");

                return _value!;
            }
        }

        private Result(T value)
        {
            _value = value;
        }

        private Result() { }

        /// <summary>
        /// Creates a new successful result with the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>A successful result.</returns>
        public static Result<T> Success(T value) => new(value);

        /// <summary>
        /// Creates a new failed result with the specified error message.
        /// </summary>
        /// <param name="error">The error message.</param>
        /// <returns>A failed result.</returns>
        public new static Result<T> Failure(string error)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(error);
            
            var result = new Result<T>();
            result._errors.Add(error);
            return result;
        }

        /// <summary>
        /// Creates a new failed result with the specified error messages.
        /// </summary>
        /// <param name="errors">The error messages.</param>
        /// <returns>A failed result.</returns>
        public new static Result<T> Failure(IEnumerable<string> errors)
        {
            ArgumentNullException.ThrowIfNull(errors);
            
            var errorList = errors.ToList();
            if (errorList.Count == 0)
            {
                throw new ArgumentException("At least one error must be provided.", nameof(errors));
            }

            var result = new Result<T>();
            result._errors.AddRange(errorList);
            return result;
        }

        /// <summary>
        /// Creates a new failed result with the specified error messages.
        /// </summary>
        /// <param name="errors">The error messages.</param>
        /// <returns>A failed result.</returns>
        public new static Result<T> Failure(params string[] errors) => Failure((IEnumerable<string>)errors);

        /// <summary>
        /// Implicitly converts a value to a successful result.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        public static implicit operator Result<T>(T value) => Success(value);
    }
} 