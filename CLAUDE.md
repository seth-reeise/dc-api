# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

dc-api is the admin API for The Divine Canine, a .NET 8.0 ASP.NET Core Web API that manages customer data using MongoDB.

## Architecture

### Solution Structure
- **Solution file**: `dc-api.sln` at root
- **Main project**: `src/AdminService/` - Contains the ASP.NET Core Web API
- **Namespace**: `dc_api` (with underscore, not hyphen)

### Key Components

**Program.cs** (`src/AdminService/Program.cs`)
- Configures MongoDB using `MongoDBSettings` bound from configuration
- Registers `ICustomerService` as a singleton
- Enables CORS with policy "corsapp" (allows all origins, methods, headers)
- Swagger enabled in development mode at `/swagger/v1/swagger.json`
- Auth0 configuration present in appsettings (Domain: dev-eoiawrgb.us.auth0.com)

**Models** (`src/AdminService/Models/`)
- `Customer` - Main domain model with MongoDB BSON attributes, stores customer and pet information
- `MongoDBSettings` - Configuration model for MongoDB connection (ConnectionURI, DatabaseName, CollectionName)

**Services** (`src/AdminService/Services/`)
- `ICustomerService` - Interface defining customer operations (CRUD + search)
- `CustomerService` - Implementation (not inspected but likely MongoDB-based)

**Controllers** (`src/AdminService/Controllers/`)
- `CustomerController` - REST API at `/api/customer` with GET, POST, PUT, DELETE endpoints
- `HealthController` - Health check endpoint (recently added)

### MongoDB Integration
- Uses MongoDB.Driver package
- Customer IDs are stored as ObjectId in MongoDB but exposed as strings in the API
- MongoDB settings configured via appsettings.json under "MongoDB" section

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

### Docker Build
```bash
docker build -t dc-api .
```

### Docker Run
```bash
docker run -p 8080:80 dc-api
```

## Configuration Requirements

### MongoDB Settings

The application expects MongoDB configuration. Default placeholder values are in `appsettings.json`:
```json
{
  "MongoDB": {
    "ConnectionURI": "mongodb://localhost:27017",
    "DatabaseName": "divine_canine",
    "CollectionName": "customers"
  }
}
```

### Secrets Management

**For local development**, use .NET User Secrets to override the connection string (never commit passwords):
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

**For production**, use environment variables:
- `MongoDB__ConnectionURI` (double underscore)
- `MongoDB__DatabaseName`
- `MongoDB__CollectionName`

## API Endpoints

- `GET /api/customer` - Get all customers
- `GET /api/customer/search?search={query}` - Search customers
- `POST /api/customer` - Create customer
- `PUT /api/customer/{id}` - Update customer first name
- `DELETE /api/customer/{id}` - Delete customer
- Health check endpoint (check HealthController for exact route)

## Important Notes

- The project was recently reorganized with a `src/` folder structure
- .csproj file has been modified recently (currently unstaged in git)
- No test projects exist in the solution currently
- CORS is wide open (allows all origins) - consider restricting in production
- Auth0 is configured but authentication/authorization middleware is not currently enabled
