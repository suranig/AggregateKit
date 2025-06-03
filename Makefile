.PHONY: build test pack clean docker-build docker-test docker-pack

# Local development commands
build:
	dotnet build

test:
	dotnet test

pack:
	dotnet pack src/AggregateKit/AggregateKit.csproj --output ./artifacts

clean:
	dotnet clean
	rm -rf ./artifacts
	rm -rf ./TestResults
	find . -name "bin" -type d -exec rm -rf {} +
	find . -name "obj" -type d -exec rm -rf {} +

# Docker commands
docker-build:
	docker-compose build build

docker-test:
	docker-compose build test
	docker-compose run --rm test

docker-pack:
	mkdir -p ./artifacts
	docker-compose build pack
	docker-compose run --rm pack

# CI pipeline helpers
ci-pipeline: build test pack

# Release helpers
release-patch:
	@echo "Bumping patch version..."
	@VERSION=$$(grep -oP '<Version>\K[^<]+' src/AggregateKit/AggregateKit.csproj); \
	MAJOR=$$(echo $$VERSION | cut -d. -f1); \
	MINOR=$$(echo $$VERSION | cut -d. -f2); \
	PATCH=$$(echo $$VERSION | cut -d. -f3 | cut -d- -f1); \
	NEW_PATCH=$$((PATCH + 1)); \
	NEW_VERSION="$$MAJOR.$$MINOR.$$NEW_PATCH"; \
	sed -i "s/<Version>[^<]*<\/Version>/<Version>$$NEW_VERSION<\/Version>/g" src/AggregateKit/AggregateKit.csproj; \
	echo "Version bumped to $$NEW_VERSION"; \
	echo "$$NEW_VERSION" > .version

release-minor:
	@echo "Bumping minor version..."
	@VERSION=$$(grep -oP '<Version>\K[^<]+' src/AggregateKit/AggregateKit.csproj); \
	MAJOR=$$(echo $$VERSION | cut -d. -f1); \
	MINOR=$$(echo $$VERSION | cut -d. -f2); \
	NEW_MINOR=$$((MINOR + 1)); \
	NEW_VERSION="$$MAJOR.$$NEW_MINOR.0"; \
	sed -i "s/<Version>[^<]*<\/Version>/<Version>$$NEW_VERSION<\/Version>/g" src/AggregateKit/AggregateKit.csproj; \
	echo "Version bumped to $$NEW_VERSION"; \
	echo "$$NEW_VERSION" > .version

release-major:
	@echo "Bumping major version..."
	@VERSION=$$(grep -oP '<Version>\K[^<]+' src/AggregateKit/AggregateKit.csproj); \
	MAJOR=$$(echo $$VERSION | cut -d. -f1); \
	NEW_MAJOR=$$((MAJOR + 1)); \
	NEW_VERSION="$$NEW_MAJOR.0.0"; \
	sed -i "s/<Version>[^<]*<\/Version>/<Version>$$NEW_VERSION<\/Version>/g" src/AggregateKit/AggregateKit.csproj; \
	echo "Version bumped to $$NEW_VERSION"; \
	echo "$$NEW_VERSION" > .version

# Create git tag based on current version
tag-version:
	@VERSION=$$(grep -oP '<Version>\K[^<]+' src/AggregateKit/AggregateKit.csproj); \
	git tag "v$$VERSION"; \
	echo "Created tag v$$VERSION"

# Push tags to remote
push-tags:
	git push --tags

# Complete release workflow
release-workflow:
	@echo "Starting release workflow..."
	@echo "1. Clean and build"
	$(MAKE) clean build
	@echo "2. Run tests"
	$(MAKE) test
	@echo "3. Tag version"
	$(MAKE) tag-version
	@echo "4. Create NuGet package"
	$(MAKE) pack
	@echo "Release workflow completed! To publish:"
	@echo "- Run 'git push' to push commits"
	@echo "- Run 'make push-tags' to push tags and trigger release" 