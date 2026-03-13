# Front Office ERP (Windows Desktop, .NET MAUI + .NET 10)

Front Office ERP starter solution built with **.NET MAUI**, **MVVM**, and **SQL Server**.

## Implemented architecture

- Windows-first MAUI target (`net10.0-windows10.0.19041.0`) with layered services.
- Entity Framework Core with SQL Server provider.
- Authentication with password hashing and active-user check.
- Role-Based Access Control (RBAC) with `Roles` + `Permissions` tables.
- Module-level visibility in dashboard navigation based on role permissions.
- Async employee, roster, and excel comparison operations.
- Pagination support for employee listing service.

## Modules included

1. Authentication & User Management (seeded admin/manager/operator users)
2. Employee Management
3. Duty Roster (shift codes G/M/E/N/O, auto-rotation, copy previous month)
4. Excel Compare (`.xlsx`, `.csv` with export to Excel/CSV/PDF)
5. Reports export service
6. Dashboard KPIs (employee total + shift counts)

## Database entities

- Users
- Roles
- Permissions
- Employees
- DutyRoster
- ExcelCompareResults
- Reports

## Main libraries

- CommunityToolkit.Mvvm
- EPPlus
- CsvHelper
- iText7
- EF Core SQL Server

## Seeded credentials

- `admin / Admin@123`
- `manager / Manager@123`
- `operator / Operator@123`
