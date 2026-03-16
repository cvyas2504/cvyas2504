# FrontOffice ERP

FrontOffice ERP is a .NET MAUI desktop starter for enterprise front-office operations, designed with a clean/modular architecture and local SQLite persistence.

## Implemented baseline

- .NET MAUI desktop client (`FrontOfficeApp`)
- EF Core + SQLite local embedded database
- Authentication and role/permission model (Admin, Manager, User)
- Modules:
  - Excel Compare
  - Duty Roster Management
  - Reports
- Export support in services (PDF, Excel, CSV)
- Shared page footer:
  - `Copyright © 2026 Develop By Chetan`

## Documentation

- Architecture/runbook: `docs/FrontOfficeERP-Architecture.md`
- Database schema: `docs/database-schema.sql`

## Run quickly (Visual Studio 2022)

1. Open `FrontOfficeApp.sln`.
2. Set `FrontOfficeApp` as startup project.
3. Build and run.
4. Database file is created automatically at app data location.
