# RestaurantApp

RESTful Web API for a restaurant menu, built with **ASP.NET Core 10** using **Clean Architecture**.

## Solution Structure

```text
RestaurantApp/
├── src/
│   ├── RestaurantApp.Domain/
│   │   └── Entities/
│   ├── RestaurantApp.Application/
│   │   ├── Common/
│   │   ├── DTOs/
│   │   ├── Interfaces/
│   │   ├── Mapping/
│   │   └── Services/
│   ├── RestaurantApp.Infrastructure/
│   │   └── Persistence/
│   │       ├── Migrations/
│   │       └── Repositories/
│   └── RestaurantApp.API/
│       ├── Controllers/
│       ├── Models/
│       ├── Program.cs
│       └── appsettings.json
├── docs/
└── RestaurantApp.slnx
```

## Architecture

- **Domain** — Entities and core business types. No dependencies on other projects.
- **Application** — DTOs, repository interfaces, application services, mapping, and business rules.
- **Infrastructure** — EF Core, `ApplicationDbContext`, repository implementations, and migrations.
- **API** — Controllers, API response models, validation configuration, OpenAPI, and Scalar.

Dependency direction:

```text
API -> Application -> Domain
API -> Infrastructure -> Application -> Domain
```

## Requirements

- .NET 10 SDK
- SQL Server
- `dotnet-ef` CLI tool

## Run

```powershell
cd D:\API\RestaurantApp
dotnet restore
dotnet ef database update --project src\RestaurantApp.Infrastructure --startup-project src\RestaurantApp.API
dotnet run --project src\RestaurantApp.API --launch-profile https
```

Open Scalar:

```text
https://localhost:7183/scalar/v1
```

## Endpoints

### Categories

- `GET /api/Category`
- `GET /api/Category/{id}`
- `POST /api/Category`
- `PUT /api/Category/{id}`
- `DELETE /api/Category/{id}`

### Menu Items

- `GET /api/MenuItem`
- `GET /api/MenuItem/{id}`
- `POST /api/MenuItem`
- `PUT /api/MenuItem/{id}`
- `DELETE /api/MenuItem/{id}`

## EF Core Commands

```powershell
# List migrations
dotnet ef migrations list --project src\RestaurantApp.Infrastructure --startup-project src\RestaurantApp.API

# Add a migration
dotnet ef migrations add MigrationName --project src\RestaurantApp.Infrastructure --startup-project src\RestaurantApp.API

# Update the database
dotnet ef database update --project src\RestaurantApp.Infrastructure --startup-project src\RestaurantApp.API
```
