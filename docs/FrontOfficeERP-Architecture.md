# FrontOffice ERP - .NET MAUI Desktop Architecture (Clean Architecture + SQLite)

This guide defines a **complete starter architecture** for a Visual Studio 2022 solution of **FrontOffice ERP** with local SQLite storage.

---

## 1) Solution Blueprint (Step-by-Step)

### Step 1 - Create solution and projects

Create a Visual Studio 2022 solution named `FrontOfficeERP` with:

```text
FrontOfficeERP.sln
├─ FrontOfficeApp                  // .NET MAUI (Windows desktop target)
├─ FrontOfficeERP.Application      // Use cases, DTOs, interfaces
├─ FrontOfficeERP.Domain           // Entities, enums, business rules
├─ FrontOfficeERP.Infrastructure   // EF Core SQLite, repositories, exports, security
└─ FrontOfficeERP.Tests            // Unit tests for services/use-cases
```

> In this repository, `FrontOfficeApp` is the runnable MAUI project. API pieces can remain optional for future synchronization use-cases, but the target runtime for this blueprint is local desktop + embedded SQLite.

### Step 2 - Enforce clean architecture dependency direction

- `Domain` has no dependencies.
- `Application` depends on `Domain` only.
- `Infrastructure` depends on `Application` + `Domain`.
- `FrontOfficeApp` depends on `Application` + `Infrastructure`.

### Step 3 - Configure local database

Use EF Core with SQLite:
- Database file: `%LocalAppData%/FrontOfficeERP/frontoffice.db`
- Migration + seed on first run.
- Add unique constraints for user names and duty roster assignment (employee + date).

### Step 4 - Add authentication and RBAC

- Login using username/password hash (PBKDF2 or BCrypt).
- Tables: `Users`, `Roles`, `Permissions`.
- Module-level action permissions (`CanView`, `CanCreate`, `CanEdit`, `CanDelete`, `CanExport`).

### Step 5 - Build module use-cases

- Excel Compare
- Duty Roster Management
- Reports & Exports (PDF/Excel/CSV)

### Step 6 - Compose MAUI UI shell

- Left navigation flyout/menu for Dashboard and modules.
- Responsive desktop-first pages.
- Shared footer on all pages:  
  **Copyright © 2026 Develop By Chetan**

---

## 2) Recommended FrontOfficeApp folder structure

```text
FrontOfficeApp/
├─ App.xaml
├─ AppShell.xaml
├─ MauiProgram.cs
├─ Data/
│  ├─ AppDbContext.cs
│  └─ DataSeeder.cs
├─ Models/
│  ├─ User.cs
│  ├─ Employee.cs
│  ├─ DutyRoster.cs
│  ├─ ExcelComparison.cs
│  ├─ Report.cs
│  └─ Enums.cs
├─ Services/
│  ├─ AuthService.cs
│  ├─ AuthorizationService.cs
│  ├─ EmployeeService.cs
│  ├─ DutyRosterService.cs
│  ├─ ExcelComparisonService.cs
│  ├─ ReportService.cs
│  └─ ExportService.cs (recommended)
├─ ViewModels/
│  ├─ LoginViewModel.cs
│  ├─ DashboardViewModel.cs
│  ├─ ExcelCompareViewModel.cs
│  ├─ DutyRosterViewModel.cs
│  ├─ ReportsViewModel.cs
│  └─ UserManagementViewModel.cs
└─ Views/
   ├─ LoginPage.xaml
   ├─ DashboardPage.xaml
   ├─ ExcelComparePage.xaml
   ├─ DutyRosterPage.xaml
   ├─ ReportsPage.xaml
   └─ UserManagementPage.xaml
```

---

## 3) Core module implementation notes

## Authentication & user management

- `AuthService` validates password hash and status (`IsActive`).
- `AuthorizationService` resolves permissions by role and module.
- `UserManagementViewModel` supports create/activate/deactivate users and role assignment.

## Excel Compare module

Use-case workflow:
1. Upload/select File 1 and File 2.
2. Parse rows into dictionaries keyed by a selected business column.
3. Compare row-by-row.
4. Tag status: `Match`, `Difference`, `MissingInFile1`, `MissingInFile2`.
5. Persist summary + row results.
6. Export to PDF/Excel/CSV.

## Duty roster module

- Shift types: `General`, `Morning`, `Evening`, `Night`.
- Table and calendar views should bind to same data source.
- Validation rule: one assignment per employee per date.
- Auto-rotation helper to fill schedule ranges.

## Reports module

- Filters: date range, user, shift, activity/module.
- Saved reports can be rendered and exported in three formats.

---

## 4) Key starter code snippets

### SQLite registration in `MauiProgram.cs`

```csharp
var dbPath = Path.Combine(FileSystem.AppDataDirectory, "frontoffice.db");
builder.Services.AddDbContext<AppDbContext>(opts =>
    opts.UseSqlite($"Data Source={dbPath}"));
```

### Duty roster uniqueness constraint

```csharp
modelBuilder.Entity<DutyRoster>()
    .HasIndex(x => new { x.EmployeeID, x.DutyDate })
    .IsUnique();
```

### Permission lookup

```csharp
public async Task<bool> HasPermissionAsync(int roleId, ModuleType module, PermissionAction action)
{
    var permission = await db.Permissions
        .FirstOrDefaultAsync(p => p.RoleID == roleId && p.Module == module);

    if (permission is null) return false;
    return action switch
    {
        PermissionAction.View => permission.CanView,
        PermissionAction.Create => permission.CanCreate,
        PermissionAction.Edit => permission.CanEdit,
        PermissionAction.Delete => permission.CanDelete,
        PermissionAction.Export => permission.CanExport,
        _ => false
    };
}
```

### Export service contract

```csharp
public interface IExportService
{
    Task<string> ExportCsvAsync<T>(IEnumerable<T> rows, string fileName);
    Task<string> ExportExcelAsync<T>(IEnumerable<T> rows, string fileName);
    Task<string> ExportPdfAsync(string title, IEnumerable<string> lines, string fileName);
}
```

---

## 5) Run instructions in Visual Studio 2022

1. Open `FrontOfficeApp.sln`.
2. Ensure **.NET MAUI workload** is installed in VS 2022.
3. Set `FrontOfficeApp` as startup project.
4. Build and run `net10.0-windows10.0.19041.0` target (or the project target in your environment).
5. On first launch:
   - SQLite DB is created.
   - Seed users/roles/permissions are inserted.
6. Sign in with seeded admin credentials and verify module navigation.

---

## 6) Enterprise readiness checklist

- Add audit trail table for login and data changes.
- Add backup/restore utility for local SQLite DB.
- Add optimistic concurrency (`RowVersion`) for roster edits.
- Add integration tests for compare and export flows.
- Add packaging/signing profile for enterprise rollout.
