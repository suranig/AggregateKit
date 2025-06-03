# Development Guide for AggregateKit

This document provides guidance for developers working on AggregateKit.

## Development Environment Setup

### Prerequisites

- .NET SDK 8.0 or higher
- Docker and Docker Compose (for containerized development)
- Make (for using the Makefile)
- Git

## Development Workflow

### Using Make Commands

The project includes a Makefile that provides useful commands for common tasks.

#### Local Development

```bash
# Build the solution
make build

# Run tests
make test

# Create NuGet package
make pack

# Clean build artifacts
make clean

# Full CI pipeline (build, test, pack)
make ci-pipeline
```

#### Using Docker

```bash
# Build using Docker
make docker-build

# Run tests in Docker
make docker-test

# Create NuGet package in Docker
make docker-pack
```

#### Release Management

```bash
# Bump patch version (1.0.0 -> 1.0.1)
make release-patch

# Bump minor version (1.0.0 -> 1.1.0)
make release-minor

# Bump major version (1.0.0 -> 2.0.0)
make release-major

# Create git tag for current version
make tag-version

# Push tags to trigger release
make push-tags

# Complete release workflow
make release-workflow
```

## Docker

The project includes Docker support for consistent build and test environments.

### Docker Compose

```bash
# Run tests
docker-compose run --rm test

# Build the solution
docker-compose run --rm build

# Create NuGet package
docker-compose run --rm pack
```

### Dockerfile

The Dockerfile includes multiple stages:

- `build`: Builds the solution
- `test`: Runs tests
- `publish`: Creates NuGet package
- `final`: Contains the built artifacts

## CI/CD Pipeline

The GitHub Actions workflow defined in `.github/workflows/ci.yml` automatically:

1. Builds the project
2. Runs tests
3. Creates NuGet package
4. Publishes to NuGet.org when a tag is pushed

### Publishing a New Release

1. Update the version in `src/AggregateKit/AggregateKit.csproj` (or use `make release-patch/minor/major`)
2. Commit your changes
3. Create a tag: `git tag v1.0.1` (or use `make tag-version`)
4. Push the changes: `git push && git push --tags` (or use `make push-tags` after pushing commits)
5. The GitHub Actions workflow will automatically publish the NuGet package

## Code Contribution Guidelines

1. Create a new branch for your feature or bugfix
2. Ensure all tests pass with `make test`
3. Add new tests for new functionality
4. Submit a pull request
5. Ensure CI passes for your pull request 