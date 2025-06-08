# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [0.2.0] - 2025-06-08

### Changed
- **BREAKING**: Upgraded target framework from .NET Standard 2.0 to .NET 8.0
- Modernized Guard class to use .NET 8 built-in methods (ArgumentNullException.ThrowIfNull, ArgumentException.ThrowIfNullOrEmpty, ArgumentException.ThrowIfNullOrWhiteSpace)
- Enhanced Entity<TId> class with IEquatable<Entity<TId>> implementation and improved equality comparisons using EqualityComparer<T>.Default
- Improved ValueObject class with IEquatable<ValueObject> implementation and System.HashCode for better hash code generation
- Modernized Result and Result<T> classes with C# 12 collection expressions, target-typed new expressions, and implicit conversion operators
- Updated AggregateRoot<TId> to use collection expressions and AsReadOnly() instead of ToImmutableList() for better performance
- Enhanced Guard methods with CallerArgumentExpression attributes for automatic parameter name detection
- Added NotNull attributes for better nullable reference types support

### Removed
- Dependency on System.Collections.Immutable package (no longer needed)

### Fixed
- Resolved CS8604 nullable reference warning in Guard.AgainstNullOrEmpty method
- Updated unit tests to match .NET 8 exception behavior (ArgumentNullException for null values in ThrowIfNullOrEmpty/ThrowIfNullOrWhiteSpace)

## [0.1.0] - 2025-06-08

### Added
- Initial release with core DDD building blocks
- Entity<TId> base class for domain entities with identity-based equality
- AggregateRoot<TId> base class for aggregate roots with domain event handling
- ValueObject base class for value objects with value-based equality
- IDomainEvent interface and DomainEventBase for domain events
- Result and Result<T> classes for functional-style error handling
- Guard utility class for argument validation
- Docker support for development and CI environments
- GitHub Actions workflow for CI/CD
- Test coverage reporting with Coverlet and Codecov