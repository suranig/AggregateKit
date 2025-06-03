.PHONY: build test test-coverage pack clean benchmark docker-build docker-test docker-test-coverage docker-pack

# Local development commands
build:
	dotnet build

test:
	dotnet test

test-coverage:
	mkdir -p ./TestResults/Coverage
	dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=cobertura /p:CoverletOutput=./TestResults/Coverage/coverage.cobertura.xml
	dotnet tool install -g dotnet-reportgenerator-globaltool || true
	reportgenerator "-reports:./TestResults/Coverage/coverage.cobertura.xml" "-targetdir:./TestResults/Coverage/Reports" "-reporttypes:Html;Badges"
	@echo "Coverage report generated at ./TestResults/Coverage/Reports/index.html"

benchmark:
	dotnet run --project benchmarks/AggregateKit.Benchmarks/AggregateKit.Benchmarks.csproj -c Release

benchmark-quick:
	dotnet run --project benchmarks/AggregateKit.Benchmarks/AggregateKit.Benchmarks.csproj -c Release -- --job short --warmupCount 1 --iterationCount 3

pack:
	dotnet pack src/AggregateKit/AggregateKit.csproj --output ./artifacts

clean:
	dotnet clean
	rm -rf ./artifacts
	rm -rf ./TestResults
	rm -rf ./BenchmarkDotNet.Artifacts
	find . -name "bin" -type d -exec rm -rf {} +
	find . -name "obj" -type d -exec rm -rf {} +

# Docker commands
docker-build:
	docker-compose build build

docker-test:
	docker-compose build test
	docker-compose run --rm test

docker-test-coverage:
	mkdir -p ./TestResults/Coverage
	docker-compose run --rm -v $(PWD)/TestResults:/app/TestResults test dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=cobertura /p:CoverletOutput=/app/TestResults/Coverage/coverage.cobertura.xml
	dotnet tool install -g dotnet-reportgenerator-globaltool || true
	reportgenerator "-reports:./TestResults/Coverage/coverage.cobertura.xml" "-targetdir:./TestResults/Coverage/Reports" "-reporttypes:Html;Badges"
	@echo "Coverage report generated at ./TestResults/Coverage/Reports/index.html"

docker-pack:
	mkdir -p ./artifacts
	docker-compose build pack
	docker-compose run --rm pack

# CI pipeline helpers
ci-pipeline: build test test-coverage pack

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
	@echo "2. Run tests with coverage"
	$(MAKE) test-coverage
	@echo "3. Run quick benchmarks"
	$(MAKE) benchmark-quick
	@echo "4. Tag version"
	$(MAKE) tag-version
	@echo "5. Create NuGet package"
	$(MAKE) pack
	@echo "Release workflow completed! To publish:"
	@echo "- Run 'git push' to push commits"
	@echo "- Run 'make push-tags' to push tags and trigger release" 