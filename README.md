# dc-api

API for the admin side of The Divine Canine.

## Configuration

This API requires MongoDB connection settings to run. The configuration uses .NET's layered system where secrets override placeholder values:

1. **Placeholder values** are in `src/AdminService/appsettings.json` (safe to commit)
2. **Real connection strings** should be set using User Secrets (never committed)

### Quick Start

Set your MongoDB connection string locally:

```bash
cd src/AdminService
dotnet user-secrets set "MongoDB:ConnectionURI" "your-mongodb-connection-string"
```

Then run the API:

```bash
dotnet run --project src/AdminService/AdminService.csproj
```

For more detailed configuration information, see [CLAUDE.md](CLAUDE.md).

