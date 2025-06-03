FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /app

# Copy csproj and restore as distinct layers
COPY *.sln .
COPY src/AggregateKit/*.csproj ./src/AggregateKit/
COPY tests/AggregateKit.Tests/*.csproj ./tests/AggregateKit.Tests/
RUN dotnet restore

# Copy everything else and build
COPY . .
RUN dotnet build --configuration Release --no-restore

# Run tests
FROM build AS test
WORKDIR /app
CMD ["dotnet", "test", "--configuration", "Release", "--no-build", "--verbosity", "normal"]

# Publish
FROM build AS publish
WORKDIR /app
RUN dotnet pack src/AggregateKit/AggregateKit.csproj --configuration Release --no-build --output ./artifacts

# Final image that can be used for both testing and publishing
FROM build AS final
WORKDIR /app
COPY --from=publish /app/artifacts/*.nupkg ./artifacts/ 