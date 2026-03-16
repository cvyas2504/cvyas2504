# FrontOffice ERP (Enterprise Client-Server Blueprint)

This repository now includes a production-style ERP blueprint with:

- `FrontOfficeApp` - .NET MAUI Desktop Client (MVVM)
- `FrontOfficeERP.API` - ASP.NET Core Web API backend (layered)
- `docs/database-schema.sql` - normalized SQL Server schema
- `docs/FrontOfficeERP-Architecture.md` - architecture and runbook

## Key capabilities delivered

- JWT authentication and role-based authorization
- Controllers/Services/Repositories/DTOs/Models architecture
- API logging + centralized exception middleware
- Duty roster duplicate-shift prevention
- Excel compare detection (added/removed/modified)
- Dashboard + About footer text:
  - Copyright © 2026 Develop By Chetan

## Suggested Visual Studio solution layout

```text
FrontOfficeERP.API
 ├── Controllers
 ├── Services
 ├── Repositories
 ├── Models
 ├── DTOs
 ├── Middleware
 └── Data

FrontOfficeERP.Client (implemented as FrontOfficeApp)
 ├── Views
 ├── ViewModels
 ├── Services
 ├── Models
 ├── Utilities
```

For complete implementation details and startup steps, see `docs/FrontOfficeERP-Architecture.md`.
