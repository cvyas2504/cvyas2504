# Front Office Management Desktop Application (.NET MAUI, .NET 10)

A modern front office management app built with **.NET MAUI** and **MVVM**, including:

- Secure role-based login (Admin / Manager / Front Office User)
- Dashboard with office KPIs and quick navigation
- Excel comparison module (exact, partial, missing, duplicate detection)
- Employee management
- Duty roster management with auto shift rotation
- Reporting and export to Excel/PDF
- SQLite persistence through EF Core

## Tech Stack

- .NET 10 (targeted)
- .NET MAUI UI
- CommunityToolkit.Mvvm
- ClosedXML for Excel processing/export
- iText7 for PDF export
- EF Core + SQLite database

## Default Seeded Users

- `admin / Admin@123` (Admin)
- `manager / Manager@123` (Manager)
- `frontdesk / Front@123` (Front Office User)

## Architecture

- MVVM with feature-specific ViewModels
- Service layer for business/domain logic
- EF Core DbContext for data operations
- Async commands and operations to avoid UI freezing during large file processing

## Main Modules

- **Login & Role Management**
- **Dashboard**
- **Excel Compare**
- **Employees**
- **Duty Roster**
- **Reports**
- **Settings**

## Notes

- Excel compare is designed for high-volume rows and processes data asynchronously.
- Passwords are hashed (SHA-256 in this baseline implementation).
- The app stores all operational data in local SQLite.
