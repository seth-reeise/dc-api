# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

dc-api is the admin API for The Divine Canine, a .NET 8.0 ASP.NET Core Web API that manages customer data using MongoDB. It provides CRUD operations for customer and pet information.

## Architecture

### Solution Structure
```
dc-api/
├── dc-api.sln                    # Solution file
├── Dockerfile                    # Docker build configuration
├── src/
│   ├── AdminService/             # Main ASP.NET Core Web API project
│   │   ├── AdminService.csproj   # Project file (targets net8.0)
│   │   ├── Program.cs            # Application entry point
│   │   ├── Controllers/          # API controllers
│   │   ├── Models/               # Domain models
│   │   ├── Services/             # Business logic services
│   │   └── appsettings.json      # Configuration
│   └── AdminService.Tests/       # Test project (xUnit)
│       ├── AdminService.Tests.csproj
│       ├── Unit/                 # Unit tests
│       │   └── CustomerControllerTests.cs
│       └── Integration/          # Integration tests
│           ├── CustomWebApplicationFactory.cs
│           ├── CustomerControllerIntegrationTests.cs
│           └── HealthControllerIntegrationTests.cs
```

### Namespaces
- **AdminService.Controllers** - API controllers
- **AdminService.Models** - Domain models
- **AdminService.Services** - Service interfaces and implementations
- **AdminService.Tests.Unit** - Unit tests
- **AdminService.Tests.Integration** - Integration tests
- **dc_api.Controllers** - Legacy namespace used in CustomerController

### Key Components

**Program.cs** (`src/AdminService/Program.cs`)
- Configures MongoDB using `MongoDBSettings` bound from configuration
- Registers `ICustomerService` as a singleton
- Enables CORS with policy "corsapp" (allows all origins, methods, headers)
- Swagger enabled in development mode at `/swagger/v1/swagger.json`
- Auth0 configuration present in appsettings but authentication middleware is NOT enabled

**Models** (`src/AdminService/Models/`)

`Customer` - Main domain model with MongoDB BSON attributes:
- `Id` - MongoDB ObjectId (stored as ObjectId, exposed as string)
- `firstName`, `lastName` - Customer name
- `phoneNumber`, `email` - Contact info
- `addressLine1`, `addressLine2`, `city`, `state`, `zipcode` - Address
- `vetName`, `vetPhoneNumber` - Veterinarian info
- `dogName`, `dogBreed`, `dogAge` - Pet info
- `notes`, `signature` - Additional data

`MongoDBSettings` - Configuration model for MongoDB connection:
- `ConnectionURI` - MongoDB connection string
- `DatabaseName` - Database name
- `CollectionName` - Collection name

**Services** (`src/AdminService/Services/`)

`ICustomerService` - Interface defining operations:
- `CreateAsync(Customer)` - Create new customer
- `GetAllCustomers()` - Get all customers
- `SearchCustomers(string)` - Search by dogName, firstName, or lastName (regex, case-insensitive, starts-with)
- `SearchCustomerById(string)` - Get customer by ID
- `UpdateFirstName(string, string)` - Update customer's first name
- `DeleteCustomer(string)` - Delete customer by ID

`CustomerService` - MongoDB-based implementation using regex search

**Controllers** (`src/AdminService/Controllers/`)
- `CustomerController` - REST API at `/api/customer`
- `HealthController` - Health check at `/health`

### NuGet Packages
- `MongoDB.Driver` (2.14.1) - MongoDB database driver
- `Swashbuckle.AspNetCore` (6.1.5) - Swagger/OpenAPI support
- `Newtonsoft.Json` (13.0.1) - JSON serialization
- `Microsoft.AspNetCore.Mvc.Testing` (6.0.8) - Testing support

## Development Commands

### Build
```bash
dotnet build dc-api.sln
```

### Run
```bash
dotnet run --project src/AdminService/AdminService.csproj
```

### Restore
```bash
dotnet restore src/AdminService/AdminService.csproj
```

### Test
```bash
# Run all tests
dotnet test dc-api.sln

# Run tests with verbose output
dotnet test dc-api.sln --verbosity normal

# Run specific test project
dotnet test src/AdminService.Tests/AdminService.Tests.csproj

# Run with coverage (requires coverlet)
dotnet test dc-api.sln --collect:"XPlat Code Coverage"
```

### Docker Build
```bash
docker build -t dc-api .
```

### Docker Run
```bash
docker run -p 8080:80 dc-api
```

**Note:** The Dockerfile uses .NET 6.0 SDK/runtime images, but the project targets .NET 8.0. Consider updating the Dockerfile to use .NET 8.0 images for consistency.

## Configuration

### MongoDB Settings

The application expects MongoDB configuration in `appsettings.json`:
```json
{
  "MongoDB": {
    "ConnectionURI": "mongodb://localhost:27017",
    "DatabaseName": "The_divine_canine",
    "CollectionName": "contract_form"
  }
}
```

### Secrets Management

**For local development**, use .NET User Secrets to override the connection string:
```bash
cd src/AdminService
dotnet user-secrets set "MongoDB:ConnectionURI" "your-actual-connection-string-here"
dotnet user-secrets set "MongoDB:DatabaseName" "your-database-name"
dotnet user-secrets set "MongoDB:CollectionName" "your-collection-name"
```

To view your secrets:
```bash
dotnet user-secrets list
```

To remove a secret:
```bash
dotnet user-secrets remove "MongoDB:ConnectionURI"
```

**For production**, use environment variables (double underscore for nested keys):
- `MongoDB__ConnectionURI`
- `MongoDB__DatabaseName`
- `MongoDB__CollectionName`

### Auth0 Configuration

Auth0 settings are present but authentication is not currently enabled:
```json
{
  "Auth0": {
    "Domain": "dev-eoiawrgb.us.auth0.com",
    "Audience": "https://divinecanine/api"
  }
}
```

## API Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/health` | Health check (returns "Healthy!") |
| GET | `/api/customer` | Get all customers |
| GET | `/api/customer/search?search={query}` | Search customers by name or dog name |
| POST | `/api/customer` | Create new customer |
| PUT | `/api/customer/{id}` | Update customer's first name |
| DELETE | `/api/customer/{id}` | Delete customer |

### Search Behavior
The search endpoint uses case-insensitive regex matching with starts-with pattern on:
- `dogName`
- `firstName`
- `lastName`

## Testing

### Test Structure
- **Unit Tests** (`src/AdminService.Tests/Unit/`) - Test controllers with mocked services using Moq
- **Integration Tests** (`src/AdminService.Tests/Integration/`) - Test HTTP endpoints using `WebApplicationFactory`

### Test Packages
- `xunit` (2.6.2) - Test framework
- `Moq` (4.20.70) - Mocking framework
- `Microsoft.AspNetCore.Mvc.Testing` (8.0.0) - Integration testing
- `coverlet.collector` (6.0.0) - Code coverage

### Test Coverage
| Component | Test File | Tests |
|-----------|-----------|-------|
| CustomerController | `Unit/CustomerControllerTests.cs` | 14 tests covering CRUD operations |
| CustomerController | `Integration/CustomerControllerIntegrationTests.cs` | 8 integration tests |
| HealthController | `Integration/HealthControllerIntegrationTests.cs` | 3 tests |

## Important Notes

- **CORS**: Wide open (allows all origins) - restrict in production
- **Authentication**: Auth0 config exists but middleware is not enabled
- **Dockerfile mismatch**: Uses .NET 6.0 images while project targets .NET 8.0
- **.NET 8.0**: Project uses modern .NET 8.0 with nullable reference types and implicit usings enabled
