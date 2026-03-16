# FrontOffice ERP - Enterprise Architecture Blueprint

## 1) System Architecture

FrontOffice ERP is designed as a secure **client-server ERP platform**:

- **Client:** .NET MAUI desktop application (`FrontOfficeApp`) using MVVM.
- **Server:** ASP.NET Core Web API (`FrontOfficeERP.API`) with layered architecture.
- **Database:** Microsoft SQL Server with EF Core.

### High-level flow

1. User logs in from MAUI client.
2. Client sends login request to `POST /api/auth/login`.
3. API validates user + password hash, issues JWT token.
4. Client includes bearer token on subsequent calls.
5. API enforces role policies (`AdminOnly`, `ManagerOrAdmin`) and permission checks.
6. Data is persisted in SQL Server via EF Core repositories/services.

## 2) Backend Layering

`FrontOfficeERP.API` is organized into:

- `Controllers`: HTTP endpoints for auth, users, duty roster, excel compare.
- `Services`: Business logic and validation.
- `Repositories`: Data access abstraction.
- `Models`: EF entities.
- `DTOs`: Request/response contracts.
- `Middleware`: API request logging and centralized exception handling.
- `Data`: `AppDbContext` and mapping constraints.
- `Auth`: JWT token + password hashing services.

## 3) Security Model

Implemented security controls:

- JWT authentication (`Bearer`).
- Role-based authorization policies.
- Password hashing via PBKDF2 (API).
- Request logging middleware.
- Global exception handling middleware.
- Duplicate-shift prevention through unique index and service validation.

## 4) Client Modules

Implemented UI modules in MAUI:

- Login
- Dashboard
- User Management
- Employee Management
- Duty Roster
- Excel Compare
- Reports
- Settings
- About

The dashboard and about include footer text:

> Copyright © 2026 Develop By Chetan

## 5) Excel Compare Module (Design)

Comparison strategy used in API service:

- Normalize each row to key-value dictionary.
- Compare by unique business key (`Key` field).
- Detect:
  - Added (only in file2)
  - Removed (only in file1)
  - Modified (present in both, values differ)
- Save summary to `ExcelCompareLogs`.

Client can upload and parse Excel using EPPlus/CsvHelper and call compare endpoint.

## 6) Reporting Module (Design)

Reports to generate from MAUI service layer:

- Employee list
- User list
- Monthly duty roster
- Shift summary
- Excel compare results

Exports:

- PDF (iText7)
- XLSX (EPPlus)
- CSV (CsvHelper)

## 7) Visual Studio Run Instructions

### Prerequisites

- Visual Studio 2022/2026 with .NET 10 workload support.
- SQL Server (Developer/Express/LocalDB).

### Run API

1. Open solution and set `FrontOfficeERP.API` startup.
2. Update `appsettings.json` connection string and JWT secret.
3. Execute EF Core migrations:
   - `Add-Migration InitialCreate`
   - `Update-Database`
4. Run API and verify Swagger.

### Run MAUI Client

1. Set `FrontOfficeApp` as startup project.
2. Update API base URL in `Services/ApiClient.cs` if needed.
3. Build and run on Windows target.
4. Login and navigate modules from dashboard sidebar.

## 8) Deployment Considerations

- Store JWT secret in secure key vault.
- Use HTTPS only + reverse proxy.
- Enable SQL backups and retention policy.
- Add structured logging sink (Serilog/Application Insights).
- Add CI/CD pipelines with test gates and migration stage.
