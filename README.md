# Weather Service

A .NET 9 Web API that retrieves weather data from an external provider, stores successful responses in SQL Server, and falls back to the latest persisted data when the external service is unavailable.

## Architecture

The solution follows a layered architecture with clear separation of concerns.

```text
WeatherService.Api
        ↓
WeatherService.Application
        ↓
WeatherService.Infrastructure
        ↓
WeatherService.Domain
```

### Projects

#### WeatherService.Api

Responsible for:

* HTTP endpoints
* Dependency injection configuration
* Swagger configuration
* Health checks

#### WeatherService.Application

Responsible for:

* Business logic
* Service orchestration
* Contracts and interfaces

#### WeatherService.Infrastructure

Responsible for:

* External API communication
* Database access
* Repository implementations
* Entity Framework Core configuration

#### WeatherService.Domain

Responsible for:

* Domain entities

---

## Design Decisions

### Repository Pattern

Used to abstract persistence logic from business logic.

### Dependency Injection

Used throughout the application for loose coupling and testability.

### Options Pattern

Used to bind weather API configuration from application settings.

### Fallback Strategy

If the external weather provider is unavailable, the service returns the latest successfully persisted weather response from SQL Server.

### Http Resilience

Implemented using:

* Microsoft.Extensions.Http.Resilience

Provides:

* Retry policies
* Timeout handling
* Resilience pipeline for external requests

---

## Technologies

* .NET 9
* ASP.NET Core Web API
* Entity Framework Core
* SQL Server
* Serilog
* xUnit
* Moq
* FluentAssertions
* Docker
* Swagger

---

## Running Locally

### Prerequisites

* .NET 9 SDK
* SQL Server

### Apply Migrations

```bash
dotnet ef database update \
--project WeatherService.Infrastructure \
--startup-project WeatherService.Api
```

### Run Application

```bash
dotnet run --project WeatherService.Api
```

---

## Docker

Build and run:

```bash
docker compose up --build
```

---

## API Endpoints

### Get Weather

```http
GET /api/weather
```

Returns:

* Latest weather from external provider
* Cached weather from database if provider is unavailable

### Health Check

```http
GET /health
```

Returns application health status.

---

## Testing

Run tests:

```bash
dotnet test
```

### Covered Scenarios

* External API succeeds
* External API fails and cached data exists
* External API fails and database access fails
* External API returns empty response

---

## Logging

Structured logging is implemented using Serilog.

Errors from:

* External provider communication
* Database operations

are logged for troubleshooting and monitoring.

---

## Future Improvements

* Integration tests
* Containerized database migrations
* CI/CD pipeline
* OpenTelemetry integration
* Distributed caching
* Multiple weather providers support

```
```
