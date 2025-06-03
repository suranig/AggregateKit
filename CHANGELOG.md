# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [0.1.0] - 2025-06-03

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
- Test coverage reporting with Coverlet 